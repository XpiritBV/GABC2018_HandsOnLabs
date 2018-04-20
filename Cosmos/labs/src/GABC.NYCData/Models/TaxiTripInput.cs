using System;
using Microsoft.Azure.Documents.Spatial;

namespace GABC.NYCData.Models
{
    public class TaxiTripInput
    {
        public Point Location { get; set; }

        public int RadiusInMeter { get; set; }

        public DateTime DateTime { get; set; }

        public TimeSpan TimeSpan { get; set; }
    }
}
