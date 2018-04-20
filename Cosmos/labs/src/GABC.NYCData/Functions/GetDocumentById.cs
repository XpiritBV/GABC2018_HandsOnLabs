using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace GABC.NYCData.Functions
{
    /// <summary>
    /// Retrieves a CosmosDB document based on the given databaseId, collectionId and documentId.
    /// </summary>
    public static class GetDocumentById
    {
        private static readonly string CosmosDbApiKey = Environment.GetEnvironmentVariable("CosmosDbApiKey");
        private static readonly string CosmosDbUri = Environment.GetEnvironmentVariable("CosmosDbUri");
        private static readonly DocumentClient DocumentClient = new DocumentClient(new Uri(CosmosDbUri), CosmosDbApiKey);

        [FunctionName(nameof(GetDocumentById))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getById/{databaseId}/{collectionId}/{documentId}")]HttpRequest req, 
            string databaseId,
            string collectionId,
            string documentId,
            TraceWriter log)
        {
            var documentUri = UriFactory.CreateDocumentUri(
                databaseId,
                collectionId,
                documentId);

            IActionResult result;

            try
            {
                ResourceResponse<Document> document = await DocumentClient.ReadDocumentAsync(documentUri);
                result = new OkObjectResult(
                    JsonConvert.SerializeObject(document.Resource, Formatting.Indented)
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
