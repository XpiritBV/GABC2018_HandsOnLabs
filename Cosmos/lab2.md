## Hands-on Lab 2

Find the taxi(s) which the murderer could have used for the murder committed at 00:06 on 24th Jan 2014.

### Step 2.1. Creating additional collections in Cosmos DB for the taxi trip data.

For the taxi trip data we will use another collection (`taxitrips`) in the existing Cosmos DB database. The Azure Function triggered by Cosmos DB requires a seperate collection (`leases`) for the [Cosmos DB changefeed](https://docs.microsoft.com/en-us/azure/cosmos-db/change-feed). This collection should be created automatically when the Function App is published but you can also create it yourself just to be sure.

1. Add two collections named `taxitrips` and `leases` to the existing `nycdatabase` in Cosmos DB with the following properties:
    - Storage: `Fixed (10GB)`
    - Throughput: `2000 RU/s`

Note that the throughput is higher than the `complaint` collection. This is because we want to import the taxi trip data at a higher rate so we don't need to wait so long. Feel free to use even higher throughputs.

Do __not__ upload the taxi trip data yet! First an Azure Function needs to be in place which will be triggered when new records will added.

### Step 2.2. Creating an Azure Function to update taxi trip documents once they are added to Cosmos DB

The NYC taxi trip record set contains geo coordinates for pickup and drop off locations but this data can't be directly queried by Cosmos DB because it is not in the [GeoJSON](https://docs.microsoft.com/en-us/azure/cosmos-db/geospatial) format. 

We will write an Azure Function that will be triggered each time a document is added (or updated) in the Cosmos DB `taxitrips` collection. The function  will add two properties (`pickup_location` & `dropoff_location`) to the document in GeoJSON format.

1. Open the `labs/src/GABC.NYCData.Labs.sln` solution and complete the [Cosmos DB trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-cosmosdb) function named `UpdateTaxiTripGeoData`.
2. Publish the Function App to Azure.
3. Verify that the function works by adding a document [(`labs/data/nyc_taxi_data_test_document.json`)](labs/data/nyc_taxi_data_test_document.json) to the `taxitrips` collection manually through the Azure portal. If this does not work then call one of the HttpTrigger functions in the Function App and retry adding a document manually. Or, if all else fails, restart the Function App and retry.

### Step 2.3 Creating a Data Factory pipeline for the taxi trip data

1. Follow the same steps as described in Lab 1 - Step 1.2 but replace the names and data file with the taxi trip data (`taxitrips\nyc_taxi_data_2014-01-29.csv`). 
2. Edit the schema of the source data by using the information in [`labs/data/nyc_taxi_data_column_types.csv`](labs/data/nyc_taxi_data_column_types.csv). 
3. Set the cloud units and parallel copies to `8`.
4. The copy action will take about 35 mins and should result in 477984 documents being copied. The CosmosDBTrigger will lagg behind a bit so it will take some time before all the documents have been updated with the GeoJSON data.

Once the pipeline is started you can spot check some documents in the `taxitrips` collection to verify if the new properties are added by the Azure Function.

### Step 2.4 Writing a geospatial query in Cosmos DB 

In Cosmos DB create a SQL query on the `taxitrips` collection to find the taxi trip record(s) where the pickup_location is within 600 meter of the murder location. The murder occured on 29th Jan 2014 at 00:06. See [`labs/queries/Cosmos_DB_queries.md`](labs/queries/Cosmos_DB_queries.md) for the query.

What is the taxi pickup time and the pickup and dropoff coordinates?

### Step 2.5 Creating an Azure Function for the spatial query

1. Open the `labs/src/GABC.NYCData.Labs.sln` solution and complete the HttpTrigger function named `GetTaxiTripsWithinRange`.
2. Run the function locally. You can use the [`labs/queries/taxitrips_function_calls.http`](labs/queries/taxitrips_function_calls.http) file when you're using VSCode with the REST client extension. 
3. Finally publish the Function App to Azure and run the function from Azure. The results should be the same as returned in Step 2.4.

<< Back to the main [readme](readme.md) || Go to [Hands-on Lab 1](lab1.md) >>