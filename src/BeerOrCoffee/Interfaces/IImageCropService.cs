using System;
namespace BeerOrCoffee.Interfaces
{
    public interface IImageCropService
    {
        byte[] CropImage(byte[] sourceFile, Xamarin.Forms.Rectangle rect);
    }
}
