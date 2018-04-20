using System;
using Newtonsoft.Json;

namespace GABC.NYCData.Models
{
    public class Complaint
    {
        public Guid Id { get; set; }

        [JsonProperty("CMPLNT_FR_DT")]
        public DateTime Date { get; set; }

        [JsonProperty("CMPLNT_FR_TM")]
        public TimeSpan Time { get; set; }

        [JsonProperty("KY_CD")]
        public string OffenseId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
