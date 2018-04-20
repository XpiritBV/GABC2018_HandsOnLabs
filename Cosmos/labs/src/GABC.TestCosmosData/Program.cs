using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GABC.TestCosmosData
{
    public class Program
    {
        private static DocumentClient _client;

        public static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            string cosmosDbUri = config["CosmosDBUri"];
            string apiKey = config["CosmosDBKey"];
            string databaseId = config["DatabaseId"];
            string collectionId = config["CollectionId"];

            Console.WriteLine($"Connecting to {cosmosDbUri} with key {apiKey}.");
            Console.WriteLine($"Using DB {databaseId} and collection {collectionId}.");

            Console.WriteLine("\nChoose one of the following actions:");
            Console.WriteLine("A - Read a document");
            Console.WriteLine("B - Add a property to a document");
            Console.WriteLine("C - Remove a property to a document");
            ConsoleKeyInfo choice = Console.ReadKey();

            Console.WriteLine("\nEnter a document ID to retrieve the document from the collection:");
            var documentId = Console.ReadLine();

            var documentUri = UriFactory.CreateDocumentUri(
                databaseId,
                collectionId,
                documentId);

            try
            {
                using (_client = new DocumentClient(new Uri(cosmosDbUri), apiKey))
                {
                    switch (choice.Key)
                    {
                        case ConsoleKey.A:
                            await ReadDocument(documentUri);
                            break;
                        case ConsoleKey.B:
                            await AddPropertyToDocument(documentUri);
                            await ReadDocument(documentUri);
                            break;
                        case ConsoleKey.C:
                            await RemovePropertyFromDocument(documentUri);
                            await ReadDocument(documentUri);
                            break;
                    }
                }
            }
            catch (DocumentClientException documentClientException)
            {
                Exception baseException = documentClientException.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", 
                    documentClientException.StatusCode, 
                    documentClientException.Message,
                    baseException.Message);
            }

            catch (Exception exception)
            {
                Exception baseException = exception.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}",
                    exception.Message, 
                    baseException.Message);
            }

            finally
            {
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
            }
        }

        private static async Task ReadDocument(Uri documentUri)
        {
            var response = await _client.ReadDocumentAsync(documentUri);
            var document = response.Resource;
            var json = JsonConvert.SerializeObject(document, Formatting.Indented);
            Console.WriteLine(json);
        }

        private static async Task AddPropertyToDocument(Uri documentUri)
        {
            var readResponse = await _client.ReadDocumentAsync(documentUri);
            dynamic document = readResponse.Resource;
            document.pickup_location = new
            {
                type = "Point",
                coordinates = new[] {document.pickup_longitude, document.pickup_latitude}
            };

            ResourceResponse<Document> replaceResponse = await _client.ReplaceDocumentAsync(document);
            var replacedDocument = replaceResponse.Resource;
            Console.WriteLine($"Document {replacedDocument.Id} succesfully updated.");
        }

        private static async Task RemovePropertyFromDocument(Uri documentUri)
        {
            var readResponse = await _client.ReadDocumentAsync(documentUri);
            dynamic document = readResponse.Resource;
            document.pickup_location = null;
            ResourceResponse<Document> replaceResponse = await _client.ReplaceDocumentAsync(document);
            var replacedDocument = replaceResponse.Resource;
            Console.WriteLine($"Document {replacedDocument.Id} succesfully updated.");
        }
    }
}
