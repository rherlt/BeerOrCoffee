using System;
using System.IO;
using Android.Graphics;
using Android.Util;
using BeerOrCoffee.Interfaces;

namespace BeerOrCoffee.Droid
{
    public class ImageCropService : IImageCropService
    {
        public byte[] CropImage(byte[] sourceFile, Xamarin.Forms.Rectangle rect)
        {
            byte[] result = null;

            // First decode with inJustDecodeBounds=true to check dimensions
            var options = new BitmapFactory.Options()
            {
                InJustDecodeBounds = false,
                InPurgeable = true,
            };

            using (var image = BitmapFactory.DecodeByteArray(sourceFile, 0, sourceFile.Length, options))
            {
                if (image != null)
                {
                    var sourceSize = new Size((int)image.GetBitmapInfo().Height, (int)image.GetBitmapInfo().Width);

                    using (var bitmapCropped = Bitmap.CreateBitmap(image, (int)rect.X, (int)rect.Y, (int)rect.Height, (int)rect.Width,  new Matrix(), true))
                    {
                        using (var outStream = new MemoryStream())
                        {
                            bitmapCropped.Compress(Bitmap.CompressFormat.Png, 100, outStream);

                            result = outStream.ToArray();
                        }
                        bitmapCropped.Recycle();
                    }

                    image.Recycle();
                }
                else
                    Log.Error(this.GetType().Name, "Image scaling failed: " + sourceFile);
            }

            return result;
        }
    }
}
