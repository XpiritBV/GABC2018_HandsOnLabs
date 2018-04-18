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
        /// Comment the FunctionName attribute in order to run the other functions locally.
        /// </summary>
        [FunctionName("UpdateTaxiTripGeoData")]
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
            log.Info($"Processing {documents.Count} documents...");

            foreach (dynamic document in documents)
            {
                document.pickup_location = new GeoJsonPoint(document.pickup_longitude, document.pickup_latitude);
                document.dropoff_location = new GeoJsonPoint(document.dropoff_longitude, document.dropoff_latitude);
                DocumentClient.ReplaceDocumentAsync(document);

                log.Info($"Updated {document.id}.");
            } 
        }
    }
}
