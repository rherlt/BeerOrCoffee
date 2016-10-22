using System;
using System.Collections.Generic;
using BeerOrCoffee.Enums;
using Newtonsoft.Json;
using System.Linq;

namespace BeerOrCoffee.Models
{
    public class Scores
    {
        [JsonProperty("anger")]
        public float Anger { get; set; }

        [JsonProperty("contempt")]
        public float Contempt { get; set; }

        [JsonProperty("disgust")]
        public float Disgust { get; set; }

        [JsonProperty("fear")]
        public float Fear { get; set; }

        [JsonProperty("happiness")]
        public float Happiness { get; set; }

        [JsonProperty("neutral")]
        public float Neutral { get; set; }

        [JsonProperty("sadness")]
        public float Sadness { get; set; }

        [JsonProperty("surprise")]
        public float Surprise { get; set; }

        internal BeerOrCoffeeType ToBeerOrCoffee ()
        {
            var maps = new List<Map> ();
            maps.Add (new Map { Value = Anger, BeerOrCoffee = BeerOrCoffeeType.Beer });
            maps.Add (new Map { Value = Contempt, BeerOrCoffee = BeerOrCoffeeType.Coffee });
            maps.Add (new Map { Value = Disgust, BeerOrCoffee = BeerOrCoffeeType.Coffee });
            maps.Add (new Map { Value = Fear, BeerOrCoffee = BeerOrCoffeeType.Beer });
            maps.Add (new Map { Value = Happiness, BeerOrCoffee = BeerOrCoffeeType.Beer });
            maps.Add (new Map { Value = Sadness, BeerOrCoffee = BeerOrCoffeeType.BeerAndCoffee });
            maps.Add (new Map { Value = Surprise, BeerOrCoffee = BeerOrCoffeeType.BeerAndCoffee});

            var result = maps.OrderByDescending (x => x.Value).ToList().First ().BeerOrCoffee;
            return result;

           
        }
    }
}