using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using GABC.NYCData.Models;

namespace GABC.NYCData.Functions
{

    /// <summary>
    /// Retrieves complaints based on the offense classification code (KY_CD attribute) and date (optional).
    /// 
    /// Note: The Microsoft.Azure.DocumentDB package is required to run convert the query data succesfully (Microsoft.Azure.Document.ServiceInterop error).
    /// </summary>
    public static class GetComplaintsByOffenseId
    {
        private static readonly string CosmosDbApiKey = Environment.GetEnvironmentVariable("CosmosDbApiKey");
        private static readonly string CosmosDbUri = Environment.GetEnvironmentVariable("CosmosDbUri");
        private static readonly DocumentClient DocumentClient = new DocumentClient(new Uri(CosmosDbUri), CosmosDbApiKey);
        private const string NycDatabase = "nycdatabase";
        private const string ComplaintsCollection = "complaints";

        [FunctionName(nameof(GetComplaintsByOffenseId))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequest req, 
            TraceWriter log)
        {

            /* 
             *  Tips:
             *  
             *  You'll need an `offenseId` and a `date` to query the complaint data.
             *  These can be part of the function route or provided as query string parameters.
             *
             *  Use DocumentClient.CreateDocumentQuery to create a query for Complaint objects.
             */

            throw new NotImplementedException();

        }
    }
}
