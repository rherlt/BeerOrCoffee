using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BeerOrCoffee.Models;
using Newtonsoft.Json;

namespace BeerOrCoffee.Services
{
    public class EmotionCognitiveSevice
    {

        public const string ApiUrl = "https://api.projectoxford.ai/emotion/v1.0/recognize";
        

        public async Task<Face[]> SendImage(byte[] imageBytes)
        {
            var client = new HttpClient();
            
            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiKey.Key);

            using (var content = new ByteArrayContent(imageBytes))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(ApiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var esr = JsonConvert.DeserializeObject<Face[]>(json);
                    return esr;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
