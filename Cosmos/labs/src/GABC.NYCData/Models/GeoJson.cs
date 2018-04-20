using Newtonsoft.Json;

namespace GABC.NYCData.Models
{
    /// <summary>
    /// When I created this class I wasn't aware of the existence of the
    /// Microsoft.Azure.Documents.Spatial.Point class :).
    /// </summary>
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
