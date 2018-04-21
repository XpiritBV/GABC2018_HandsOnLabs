## Hands-on Lab 1

Find out how many murders have been committed on the 29th of Jan 2014.

### Step 1.1. Creating a Cosmos DB account

Let's create a Cosmos DB account to store the complain data.

1. Use the Azure portal to create a new Cosmos DB account in the `West-Europe` region and provide a meaningfull name such as `gabc-nyc-cosmosdb`.
2. Add a database & collection with the following properties:
    - Database: `nycdatabase`
    - Collection: `complaints`
    - Storage: `Fixed (10GB)`
    - Throughput: `1000 RU/s`

### Step 1.2 Creating a Data Factory Pipeline for the complaint data

Now let's configure the Data Factory pipeline in order to transfer the csv data to Cosmos DB.

1. Use the Azure portal to create a new Data Factory instance with the following settings:
    - Version: `V2`
    - Location: `West Europe`
2. Open the new Data Factory instance and select the `Author & Monitor` quick link. 
3. Choose the `Copy Data` step.
4. Properties:
    - Enter a meaningfull name such as `Complaints to Cosmos`
5. Source data store:
    - Source data store: `Azure Blob storage`-> Next
6. Specify the Azure Blob storage account
    - Connection name: leave as default
    - Network environment: `Public network in Azure environment`
    - Account selection method: `Enter manually`
    - Storage account name: (Provide account name given during the workshop.)
    - Storage account key: (Provide key given during the workshop.) -> Next
7. Choose the input file or folder:
    - Browse and select the blob `complaints\NYPD_Complaint_Data_2014_Jan.csv` -> Choose -> Next
8. File format settings:
    - File Format: `text`
    - Delimiter: `Comma (,)`
    - Row limiter: `Carriage return + Line feed`
    - Skip line count: leave empty
    - The first data row contains colomn names: `check`
    - Treat empty column value as null: `check`
    - Schema -> Edit: Check [`labs/data/NYPD_Complaint_Data_Column_Descriptions.csv`](labs/data/NYPD_Complaint_Data_Column_Description.csv) for the correct data types. -> Next
9. Destination:
    - Destination data store: `Azure Cosmos DB` -> Next
10. Specify Azure Cosmos DB (NoSQL) connection:
    - Connection name: leave as default
    - Network environment: `Public network in Azure environment`
    - Account selection method: `From Azure subscriptions`
    - Subscription: Select your subscription
    - Cosmos DB account name: Enter the name of the created Cosmos DB instance
    - Database name: Enter the name of the created database -> Next
11. Table mapping:
    - Select table: `complaints` 
    - Nesting separator: leave default -> Next
12. Schema mapping:
    - Apply column mapping: `uncheck` -> Next
13.  Settings:
     - Fault tolerance settings: leave default
     - Advanced:
        - Cloud units: `4`
        - Parallel copies: `4` -> Next
14. Summary -> Next
15. Deployment -> Monitor. The copy action will take approx 7 minutes and should copy 39219 complaint records into Cosmos DB.
16. Go to the Cosmos DB instance and navigate to the complaints collection. Create a SQL query to find the complaint records which correspond to a murder on 29th Jan 2014. In the next step you will create an Azure Function that can retrieve this data through an HTTP endpoint.

### Step 1.3. Creating a Function App with an HttpTrigger function to return complaint data

Now let's create an Azure Function to retrieve the data from Cosmos DB.

1. Use the Azure portal to create a new Function App on a consumption plan in West Europe. We won't use the portal to write the function code there. We'll do that in Visual Studio.
2. Open the `labs/src/GABC.NYCData.Labs.sln` solution and complete the [HttpTrigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook) function named `GetComplaintsByOffenseId` to query the complaint records matching a given offense code and date. 
3. Rename `local.settings.json.example` to `local.settings.json` and fill in these Cosmos DB related settings which you can find in the CosmosDB account you created in Step 1.1:
    - `CosmosDbApiKey`
    - `CosmosDbUri`
    - `CosmosDbConnection`
4. Run the function locally to check if the function connects to the Cosmos DB instance and returns data. 
    - If you're using VSCode with the REST Client you can use the [`labs/queries/complaints_function_calls.http`](labs/queries/complaints_function_calls.http) file to call the function.
5. Publish the function to Azure. 
    - Click _Yes_ if you get a message about updating the application setting for FUNCTIONS_EXTENSION_VERSION to "beta". This is because we're using the v2 preview of Azure Functions.
6. Add the `CosmosDbApiKey`, `CosmosDbUri` and `CosmosDbConnection` settings to the function Application Settings in the Azure portal.
7. Run the function hosted in Azure and retrieve the records which correspond to murders committed on the 29th of Jan 2014. 

How many murders were committed on 29th Jan 2014?

4 Murders - Just Like Below :)

[
  {
    "Id": "81b28796-82a9-4b14-a145-0d696433f6df",
    "CMPLNT_FR_DT": "2014-01-29T00:00:00",
    "CMPLNT_FR_TM": "21:30:00",
    "KY_CD": "101",
    "Latitude": 40.687408886,
    "Longitude": -73.791248699
  },
  {
    "Id": "d00390ff-f38e-4c19-8e66-8896d769852f",
    "CMPLNT_FR_DT": "2014-01-29T00:00:00",
    "CMPLNT_FR_TM": "17:10:00",
    "KY_CD": "101",
    "Latitude": 40.652727946,
    "Longitude": -73.948321553
  },
  {
    "Id": "f02f56dc-e5b6-458d-8a04-16c5868cdf1a",
    "CMPLNT_FR_DT": "2014-01-29T00:00:00",
    "CMPLNT_FR_TM": "00:06:00",
    "KY_CD": "101",
    "Latitude": 40.764803027,
    "Longitude": -73.882309442
  },
  {
    "Id": "deaab326-1fe9-4846-93cd-7b9054321ed9",
    "CMPLNT_FR_DT": "2014-01-29T00:00:00",
    "CMPLNT_FR_TM": "00:06:00",
    "KY_CD": "101",
    "Latitude": 40.764803027,
    "Longitude": -73.882309442
  }
]

<< Back to the main [readme](readme.md) || Continue with [Hands-on Lab 2](lab2.md) >>
 
