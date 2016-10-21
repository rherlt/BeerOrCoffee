using System;
using System.IO;
using System.Threading.Tasks;
using BeerOrCoffee.ViewModels;
using Xamarin.Forms;

namespace BeerOrCoffee
{
    public partial class UserResultCell : ContentView
    {
        private bool _shouldAnimate;

        public UserResultCell()
        {
            InitializeComponent();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent != null)
                StartAnimation().ConfigureAwait(false);
            else
                StopAnimation();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                var userItem = BindingContext as UserResultItem;

                UserImage.Source = ImageSource.FromStream(() => new MemoryStream(userItem.UserImage));

                var fileName = userItem.Type == Enums.BeerOrCoffeeType.Beer ? "beer.png" : (userItem.Type == Enums.BeerOrCoffeeType.Coffee ? "coffee.png" : "BeerAndCoffee.png");

                BeerOrCoffeeImage.Source = ImageSource.FromFile(fileName);
            }

        }

        private async Task StartAnimation()
        {
            _shouldAnimate = true;

            while (_shouldAnimate)
            {
                try
                {
                    await BeerOrCoffeeImage.FadeTo(0, 1000);
                    await BeerOrCoffeeImage.FadeTo(1, 1000);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
        }

        private void StopAnimation()
        {
            _shouldAnimate = false;
        }
    }
}
