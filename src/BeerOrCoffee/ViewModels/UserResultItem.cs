using System;
using BeerOrCoffee.Enums;

namespace BeerOrCoffee.ViewModels
{
    public class UserResultItem : NotifiyingViewModel
    {
        public BeerOrCoffeeType Type
        {
            get;
            set;
        }

        public byte[] UserImage
        {
            get;
            set;
        }
    }
}
