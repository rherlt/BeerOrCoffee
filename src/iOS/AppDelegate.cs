using System;
using System.Collections.Generic;
using System.Linq;
using BeerOrCoffee.Interfaces;
using Foundation;
using UIKit;

namespace BeerOrCoffee.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Xamarin.Forms.DependencyService.Register<IImageCropService, ImageCropService>();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
