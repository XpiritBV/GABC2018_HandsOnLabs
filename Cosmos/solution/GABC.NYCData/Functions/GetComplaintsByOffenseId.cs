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
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getComplaintsByOffenseId/{offenseId}")]HttpRequest req, 
            string offenseId,
            TraceWriter log)
        {
            var queryParameters = req.GetQueryParameterDictionary();
            queryParameters.TryGetValue("date", out string dateString);
            DateTime.TryParse(dateString, out DateTime date);

            var complaintsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                NycDatabase,
                ComplaintsCollection);

            IActionResult result;

            try
            {
                var complaints = DocumentClient.CreateDocumentQuery<Complaint>(complaintsCollectionUri)
                    .Where(complaint => complaint.OffenseId == offenseId)
                    .Where(complaint => string.IsNullOrEmpty(dateString) || complaint.Date == date)
                    .AsEnumerable();

                result = new OkObjectResult(
                    JsonConvert.SerializeObject(complaints, Formatting.Indented)
                );
            }
            catch (Exception e)
            {
                result = new BadRequestObjectResult(e);
            }

            return result;
        }
    }
}
