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
            using (var sourceImage = UIImage.LoadFromData(NSData.FromArray(sourceFile)))
            {
                var cgSourceImage = sourceImage.CGImage;

                var iosRect = new CGRect(rect.X, rect.Y, rect.Right, rect.Bottom);
                var croppedCgImage = cgSourceImage.WithImageInRect(iosRect);

                using (var croppedImage = UIImage.FromImage(croppedCgImage))
                {
                    return croppedImage.AsPNG().ToArray();
                }
            }
        }
    }
}
