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
                using (var rotatedImage = ScaleAndRotateImage(sourceImage, UIImageOrientation.Right))
                {

                    var cgSourceImage = rotatedImage.CGImage;

                    var iosRect = new CGRect(rect.X, rect.Y, rect.Width, rect.Height);
                    var croppedCgImage = cgSourceImage.WithImageInRect(iosRect);

                    using (var croppedImage = UIImage.FromImage(croppedCgImage))
                    {
                        return croppedImage.AsPNG().ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Scales the and rotate image.
        /// https://forums.xamarin.com/discussion/19778/uiimage-rotation-and-transformation
        /// </summary>
        /// <returns>The and rotate image.</returns>
        /// <param name="imageIn">Image in.</param>
        /// <param name="orIn">Or in.</param>
        private UIImage ScaleAndRotateImage(UIImage imageIn, UIImageOrientation orIn)
        {
            //int kMaxResolution = 2048;

            CGImage imgRef = imageIn.CGImage;
            float width = imgRef.Width;
            float height = imgRef.Height;
            CGAffineTransform transform = CGAffineTransform.MakeIdentity();
            CGRect bounds = new CGRect(0, 0, width, height);

            //if (width > kMaxResolution || height > kMaxResolution)
            //{
            //    float ratio = width / height;

            //    if (ratio > 1)
            //    {
            //        bounds.Width = kMaxResolution;
            //        bounds.Height = bounds.Width / ratio;
            //    }
            //    else
            //    {
            //        bounds.Height = kMaxResolution;
            //        bounds.Width = bounds.Height * ratio;
            //    }
            //}

            nfloat scaleRatio = bounds.Width / width;
            var imageSize = new CGSize(width, height);
            UIImageOrientation orient = orIn;
            nfloat boundHeight;

            switch (orient)
            {
                case UIImageOrientation.Up:                                        //EXIF = 1
                    transform = CGAffineTransform.MakeIdentity();
                    break;

                case UIImageOrientation.UpMirrored:                                //EXIF = 2
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    break;

                case UIImageOrientation.Down:                                      //EXIF = 3
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
                    break;

                case UIImageOrientation.DownMirrored:                              //EXIF = 4
                    transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
                    transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
                    break;

                case UIImageOrientation.LeftMirrored:                              //EXIF = 5
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Left:                                      //EXIF = 6
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.RightMirrored:                             //EXIF = 7
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Right:                                     //EXIF = 8
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                default:
                    throw new Exception("Invalid image orientation");
                    break;
            }

            UIGraphics.BeginImageContext(bounds.Size);

            CGContext context = UIGraphics.GetCurrentContext();

            if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
            {
                context.ScaleCTM(-scaleRatio, scaleRatio);
                context.TranslateCTM(-height, 0);
            }
            else
            {
                context.ScaleCTM(scaleRatio, -scaleRatio);
                context.TranslateCTM(0, -height);
            }

            context.ConcatCTM(transform);
            context.DrawImage(new CGRect(0, 0, width, height), imgRef);

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }
    }
}
