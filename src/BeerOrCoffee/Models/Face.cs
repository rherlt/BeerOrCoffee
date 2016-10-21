using Newtonsoft.Json;

namespace BeerOrCoffee.Models
{
  
    public class Face
    {
        [JsonProperty("faceRectangle")]
        public Facerectangle FaceRectangle { get; set; }

        [JsonProperty("scores")]
        public Scores Scores { get; set; }
    }
}