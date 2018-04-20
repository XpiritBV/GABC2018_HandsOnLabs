# Global Azure Bootcamp 2018 - Cosmos DB

If you're not physically present at Xebia Amsterdam during the Global Azure bootcamp and have questions about the labs then please contact [Marc Duiker](https://twitter.com/marcduiker).

## Functional objectives

1. Find out how many murders have been committed on the 29th of Jan 2014.
2. Find the taxi(s) which the murderer could have used for the murder committed at 00:06 29th Jan 2014.

## Technical objectives

1. Learn how to create a Cosmos DB account & collections.
2. Learn how to import data into Cosmos DB using a Data Factory pipeline.
3. Learn how to query Cosmos DB data in the Azure portal.
4. Learn how to write Azure Functions to query and update data in Cosmos DB.

## Prerequisites

- VS2017 (15.6.3) with Azure Development workload
- VS Extension: Azure Functions & Web Job Tools (15.0.40405.0)
- [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) (part of Azure SDK or seperate download)
- Recommended: VSCode with REST Client extension (Postman can also be used)

## Data sets

The following files are available on Azure blob storage (credentials will be provided during the workshop):

- Lab 1: 
    - `complaints\NYPD_Complaint_Data_2014_Jan.csv`
    - `complaints\NYPD_Complaint_Data_2014_Jan_first10lines.csv`
- Lab 2: 
    - `taxitrips\nyc_taxi_data_2014-01-29.csv`
    - `taxitrips\nyc_taxi_data_2014-01-29_first10lins.csv`

The two files marked with `first10lines` can be used as test data before starting the import of the larger data sets.

## Hands-on Labs

- [Lab 1](lab1.md): Transferring the complaint data to Cosmos DB using Data Factory and write an Azure Function to query the data. 
- [Lab 2](lab2.md): Transferring the taxi trip data to Cosmos DB using Data Factory and write two Azure Functions; one to update a taxi trip document once it is inserted in Cosmos DB and one retrieve data using a geospatial query. 
_Note that this data set is large and takes about 30+ minutes to copy to Cosmos DB._

The labs can be done independently of one another.