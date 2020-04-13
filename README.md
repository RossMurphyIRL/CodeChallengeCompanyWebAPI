# CodeChallengeCompanyWebAPI
Design and code a WebAPI solution in C# for a middle tier "Company API."

## Changes made from feedback
Validation and Data Access have been abstracted out to their own layer.  
Repository pattern implemented as part of SOLID Principles being applied.  
Await used instead of result. Microsoft recommends: https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming 
Errors now return correct status codes: Client request validation returns 4xx error and server error hides details returns 5xx error.  
Tests added to ensure each layer is unit tested correctly, mocks have been added instead of using in-memory database. Library added to test async database calls.   

## Requirements
Create a Company record specifying the Name, Stock Ticker, Exchange, Isin, and optionally a website
url. You are not allowed create two Companies with the same Isin. The first two characters of an ISIN
must be letters / non numeric.

Retrieve an existing Company by Id

Retrieve a Company by ISIN

Retrieve a collection of all Companies

Update an existing Company

## Prerequisites
Visual Studio 2017/2019 with latest updates for opening the solution with asp.net core 3.1.  
Have Node.Js > 12.16.1 installed to run the client application for angular 8 to build.  
(Optional)MSSQL Server running and SSMS installed for running SQL scripts.  

## Application Setup for Running
Open open the .sln file in Visual Studio.  
Before running the application open up the CodeChallenge_Sql text file and run the statements according to how they are numbered, do not run them all together.

Inside the appsettings.json file, set the server variable in the connection string to point to you local MSSQL server.
Optional you can change the UseInMemoryDB property in the appsettings.json file to true and the application will use an in-memory database instead if you dont have sql server running.

Before starting the application please make sure you have both the WebAPI and Client Project set start up in the solutions properties settings.

## Functionality
API is built using C# with  asp.net core 3.1  
The endpoints of the API can be reached:  
// GET: api/Companies - Gets list of companies  
// GET: api/Companies/&lt;id&gt; - Gets specific company  
// GET: api/Companies/GetCompanyByIsin?isin=&lt;isin&gt; - Gets specific company with specific Isin  
// PUT: api/Companies/&lt;id&gt; - Updates specific company  
// POST: api/Companies - Create a new company  
// DELETE: api/Companies/&lt;id&gt; - Deletes specific company  
  
### Client Application
Provides a simple interface for quering the API using Angular 8.

## Authentication
This is done using bearer tokens. When the client first launched it sends a username and password to API requesting a token,
the token is then appended to the header of future requests by the client application to the Company API

## Testing
The solution provides 20 server side units for testing of the API.

## Possible Issues
Can't connect to database: In SSMS make sure in security in the server properties settings you have SQL Server and Windows Authentication selected for the server authentication.

One of the projects not loading in VS: If this happens remove and re-add the existing project back into the solution.  

IIS Express not starting: Delete vs\CodeChallenege\config\applicationhost.config and startup ports for project before running. 

## Further Improvements
Connection string should not be stored in the application but in a KeyVault.  
More validation added in regard to string length.
