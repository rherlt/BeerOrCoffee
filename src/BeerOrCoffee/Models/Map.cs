using System;
using BeerOrCoffee.Enums;

namespace BeerOrCoffee.Models
{
    public class Map
    {
      public float Value {
            get;
            set;
        }

        public BeerOrCoffeeType BeerOrCoffee {
            get;
            set;
        }
    }
}
