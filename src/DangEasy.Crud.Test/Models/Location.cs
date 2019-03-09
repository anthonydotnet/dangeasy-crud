using Newtonsoft.Json;

namespace DangEasy.UptownFunc.Test.Unit.Models
{
    public class Location
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}