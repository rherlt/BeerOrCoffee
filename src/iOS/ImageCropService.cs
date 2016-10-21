using System;
using System.IO;
using System.Runtime.InteropServices;
using BeerOrCoffee.Interfaces;
using CoreGraphics;
using Foundation;
using UIKit;

namespace BeerOrCoffee.iOS
{
    /// <summary>
    /// Image crop service.
    /// https://stackoverflow.com/questions/25265686/xamarin-forms-resize-camera-picture/25268405#25268405
    /// </summary>
    public class ImageCropService : IImageCropService
    {
        public byte[] CropImage(byte[] sourceFile, Xamarin.Forms.Rectangle rect)
        {
            using (UIImage sourceImage = UIImage.LoadFromData(NSData.FromArray(sourceFile)))
                {
                var sourceSize = sourceImage.Size;

                UIGraphics.BeginImageContextWithOptions(new CGSize((float)rect.Width, (float)rect.Height), true, 1.0f);

                sourceImage.Draw(new CGRect(rect.X, rect.Y, (float)rect.Width, (float)rect.Height));

                var resultImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                byte[] croppedBytes;

                using (NSData imageData = resultImage.AsPNG())
                {
                    croppedBytes = new byte[imageData.Length];
                    Marshal.Copy(imageData.Bytes, croppedBytes, 0, Convert.ToInt32(imageData.Length));
                }
                return croppedBytes;
            }
        }
    }
}
