using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using GABC.NYCData.Models;

namespace GABC.NYCData.Functions
{
    public static class UpdateTaxiTripGeoData
    {
        private static readonly string CosmosDbApiKey = Environment.GetEnvironmentVariable("CosmosDbApiKey");
        private static readonly string CosmosDbUri = Environment.GetEnvironmentVariable("CosmosDbUri");
        private static readonly DocumentClient DocumentClient = new DocumentClient(new Uri(CosmosDbUri), CosmosDbApiKey);
        private const string NycDatabase = "nycdatabase";
        private const string TaxiTripsCollection = "taxitrips";
        private const string LeasesCollection = "leases";

        /// <summary>
        /// This function will add pickup_location and dropoff_location properties 
        /// to a taxitrip document in GeoJSON format.
        /// </summary>
        [FunctionName(nameof(UpdateTaxiTripGeoData))]
        public static void Run(
            [CosmosDBTrigger(
                NycDatabase, 
                TaxiTripsCollection, 
                ConnectionStringSetting = "CosmosDbConnection",
                CreateLeaseCollectionIfNotExists = true,
                LeaseCollectionName = LeasesCollection
            )]
            IReadOnlyList<Document> documents,
            TraceWriter log)
        {

            /*
             * Tips:
             * 
             * You can use the dynamic type for the document 
             * in order to add new properties easily.
             * 
             * Be careful not to always add the properties 
             * since that will result in an endless update loop.
             * 
             */
        }
    }
}
