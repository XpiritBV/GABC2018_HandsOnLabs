
using System;
using System.IO;
using System.Linq;
using GABC.NYCData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Spatial;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace GABC.NYCData.Functions
{
    public static class GetTaxiTripsWithinRange
    {
        private static readonly string CosmosDbApiKey = Environment.GetEnvironmentVariable("CosmosDbApiKey");
        private static readonly string CosmosDbUri = Environment.GetEnvironmentVariable("CosmosDbUri");
        private static readonly DocumentClient DocumentClient = new DocumentClient(new Uri(CosmosDbUri), CosmosDbApiKey);
        private const string NycDatabase = "nycdatabase";
        private const string TaxiTripsCollection = "taxitrips";


        /// <summary>
        /// This function will retrieve taxi trip data the given range, location and date.
        /// </summary>
        [FunctionName(nameof(GetTaxiTripsWithinRange))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "getTaxiTripsWithinRange")]HttpRequest req, 
            TraceWriter log)
        {

            /* 
             * Tips:
             * 
             * The function requires:
             *  - a location (of type Microsoft.Azure.Documents.Spatial.Point)
             *  - a distance range (in meters)
             *  - a date/time
             *  - a timespan
             *  
             *  This information can be provided as JSON in the body of the request 
             *  and needs to be deserialized in this function.
             *  
             *  
             *  The Distance method on the Microsoft.Azure.Documents.Spatial.Point object
             *  can be used in the query to find the taxi trips with nearby pickup locations.
             *  
             */

            throw new NotImplementedException();
        }
    }
}
