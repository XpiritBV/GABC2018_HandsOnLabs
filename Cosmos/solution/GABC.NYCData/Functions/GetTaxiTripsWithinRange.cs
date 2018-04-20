
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
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName(nameof(GetTaxiTripsWithinRange))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "getTaxiTripsWithinRange")]HttpRequest req, 
            TraceWriter log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var input = JsonConvert.DeserializeObject<TaxiTripInput>(requestBody);
            
            var taxiTripsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                NycDatabase,
                TaxiTripsCollection);

            IActionResult result;

            try
            {
                var taxiTrips = DocumentClient.CreateDocumentQuery<TaxiTrip>(taxiTripsCollectionUri)
                    .Where(trip => trip.PickupLocation.Distance(input.Location) < input.RadiusInMeter)
                    .Where(
                        trip => trip.PickupDateTime >= input.DateTime.Add(-input.TimeSpan) &&
                                trip.PickupDateTime <= input.DateTime.Add(input.TimeSpan))
                    .AsEnumerable();

                result = new OkObjectResult(
                    JsonConvert.SerializeObject(taxiTrips, Formatting.Indented)
                );
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                result = new BadRequestObjectResult(e);
            }

            return result;
        }
    }
}
