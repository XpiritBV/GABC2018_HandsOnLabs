# Global Azure Bootcamp 2018 - Cosmos DB

## Functional objectives

1. Find out how many murders have been committed on the 29th of Jan 2014
2. Find the taxi(s) which the murderer could have used for the murder committed at 00:06.

## Technical objectives

1. Learn how to create collections in Cosmos DB
2. Learn how to import data into Cosmos DB using a Data Factory Pipeline.
3. Learn how to query Cosmos DB data in the Azure portal.
4. Learn how to write Azure Functions to query and update data in Cosmos DB.

## Prerequisites

- VS2017 (15.6.3) with Azure Development workload
- VS Extension: Azure Functions & Web Job Tools (15.0.40405.0)
- [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) (part of Azure SDK or seperate download)
- Recommended: VSCode with REST Client extension (Postman can also be used)

## Data sets

The following files are available on Azure blob storage (credentials will be provided during the workshop):

- `complaints\NYPD_Complaint_Data_2014_Jan.csv`
- `complaints\NYPD_Complaint_Data_2014_Jan_first10lines.csv`
- `taxitrips\nyc_taxi_data_2014-01-29.csv`
- `taxitrips\nyc_taxi_data_2014-01-29_first10lins.csv`

The two files marked with `first10lines` can be used as test data before starting the import of the larger data sets.

## Hands-on Lab 1

### Step 1.1. Creating a Cosmos DB & Data Factory instance

1. Use the Azure portal to create a new Cosmos DB instance in the `West-Europe` region.
2. Add a collection with the following properties:
    - Database: `nycdatabase`
    - Collection: `complaints`
    - Storage: `Fixed (10GB)`
    - Throughput: `1000 RU/s`
## Step 1.2 Creating a Data Factory Pipeline for the complaint data

1. Create a new Data Factory instance with the following settings:
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

How many murders were committed on 29th Jan 2014?

### Step 1.3. Creating an Azure Function to return complaint data

1. Use the Azure portal to create a Function App on a consumption plan.
2. In Visual Studio create an new Azure Function App project with an HttpTrigger to query the complaint data by passing in the offense category code and the date so the function returns records matching the offense code and date. Rename `local.settings.json.example` to `local.settings.json` and fill in these Cosmos DB related settings:
    - `CosmosDbApiKey`
    - `CosmosDbUri`
    - `CosmosDbConnection`
3. Run the function locally to check if the function connects to the Cosmos DB instance and returns the correct data. 
    - If you're using VSCode with the REST Client you can use the [`labs/queries/function_calls.http`](labs/queries/function_calls.http) file to call the function.
4. Publish the function to Azure. 
    - Click _Yes_ if you get a message about updating the application setting for FUNCTIONS_EXTENSION_VERSION to "beta".
5. Add the `CosmosDbApiKey`, `CosmosDbUri` and `CosmosDbConnection` settings to the function Application Settings in the Azure portal.
6. Run the function hosted in Azure and retrieve the records which correspond to murders committed on the 29th of Jan 2014. 

## Hands-on Lab 2

### Step 2.1. Creating additional collections in Cosmos DB for the taxi trip data.

1. Add two collections named `taxitrips` and `leases` to the existing `nycdatabase` Cosmos DB with the following properties:
    - Storage: `Fixed (10GB)`
    - Throughput: `2000 RU/s`

Do __not__ upload the taxi trip data yet! First an Azure Function need to be in place which will be triggered when new records will added (Step 2.2).

### Step 2.2. Creating an Azure Function to update taxi trip documents once they are added to Cosmos DB

The NYC taxi trip record set contains geo coordinates for pickup and drop off locations but this data can't be directly queried by Cosmos DB because it is not in the [GeoJSON](https://docs.microsoft.com/en-us/azure/cosmos-db/geospatial) format. 

We will create an Azure Function that will be triggered each time a document is added (or updated) in the Cosmos DB `taxitrips` collection. The function  will add two properties (`pickup` & `dropoff`) to the document in GeoJSON format.

1. Add a new [Cosmos DB trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-cosmosdb) function to the FunctionApp in VisualStudio that will update the document.
2. Publish the FunctionApp to Azure.
3. Verify that the function works by manually adding a document [(`labs/data/nyc_taxi_data_test_document.json`)](labs/data/nyc_taxi_data_test_document.json) to the `taxitrips` collection. If this does not work then call one of the HttpTrigger functions in the Function App and retry adding a document manually. Or if all else fails, restart the Function App.

### Step 2.3 Creating a Data Factory Pipeline for the taxi trip data

1. Follow the same steps as described in Step 1.2 but replace the complaint data with the taxi trip data blob (`taxitrips\nyc_taxi_data_2014-01-29.csv`). 
2. Edit the schema of the source data by using the information in [`labs/data/nyc_taxi_data_column_types.csv`](labs/data/nyc_taxi_data_column_types.csv). 
3. Set the cloud units and parallel copies to `8`. 
4. The copy action will take about 30 mins and should result in 477984 documents being copied. The CosmosDBTrigger will lagg behind so it will take quite some time before all the documents have been updated with the GeoJSON data.

Once the pipeline is started you can spot check some documents in the `taxitrips` collection to verify if the new properties are added by the Azure Function.

### Step 2.4 Write a geospatial query in Cosmos DB 

In Cosmos DB create a SQL query on the `taxitrips` collection to find the trip records which correspond to geo coordinates from the murder record(s). See [`labs/queries/Cosmos_DB_queries.md`](labs/queries/Cosmos_DB_queries.md) for the query.

