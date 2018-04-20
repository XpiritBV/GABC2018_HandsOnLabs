using System;
using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;

namespace GABC.NYCData.Models
{
    public class TaxiTrip
    {
        [JsonProperty("pickup_datetime")]
        public DateTime PickupDateTime { get; set; }

        [JsonProperty("dropoff_datetime")]
        public DateTime DropoffDateTime { get; set; }

        [JsonProperty("pickup_location")]
        public Point PickupLocation { get; set; }

        [JsonProperty("dropoff_location")]
        public Point DropoffLocation { get; set; }
    }
}
