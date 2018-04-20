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
                // Only add the GeoJson data when it's not yet added to the document 
                // to prevent an endless update loop.
                if (document.GetType().GetProperty("pickup_location") == null)
                {
                    document.pickup_location = new GeoJsonPoint(document.pickup_longitude, document.pickup_latitude);
                    document.dropoff_location = new GeoJsonPoint(document.dropoff_longitude, document.dropoff_latitude);
                    DocumentClient.ReplaceDocumentAsync(document);

                    log.Info($"Updated {document.id}.");
                }
            } 
        }
    }
}
