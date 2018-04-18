using Newtonsoft.Json;

namespace GABC.NYCData.Models
{
    public class GeoJsonPoint
    {
        public GeoJsonPoint(double longitude, double latitude)
        {
            Type = "Point";
            Coordinates = new[] {longitude, latitude};
        }

        [JsonProperty("type")]
        public string Type { get; }

        [JsonProperty("coordinates")]
        public double[] Coordinates { get; }
    }
}
