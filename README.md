# card-cost-api

The Card Cost API is a simple .NET Core CRUD api integrating with 3rd party services.

It is consisted by 2 controllers.
- CardCostController 
- CardCostConfigurationController

The first one accepts as input Issuer Identification Numbers (IIN), previously known as Bank Identification Number (BIN), leverages  ​https://binlist.net/ 
using <strong>IHttpClientFactory</strong> and returns the calculated cost based on the country the card belongs to.

The second one provides full CRUD operationson the card cost configuration.

**Note**: The api preloads with 2 records country:cost - GR:10 and US:15. If requested bin number doesnt belong to any configured country there is a default cost of 10 in DefaultCardCostSettings section in appsettings.json. The DefaultCardCostSettings is loaded in the system using the <strong>IOptions</strong> pattern.
In any case where ​https://binlist.net/ fails to respond with a a succcess status code a custom exception <strong>ExternalServiceCommunicationException</strong> will be thrown including status code details and optional inner exception details. The custom exceptions should be handled in the controllers.

The api can be accessed and tested by hitting the run button on VS. (setting the CardCostApi.Web as startup project is required.)

The browser will load https://localhost:44313/docs (unless launchsettings.json is modified) where swagger UI can be used to interact with the api.

# Integration testing using in memory db per test class

Integration tests ensure that an app's components function correctly at a level that includes the app's supporting infrastructure, 
such as the database, file system, and network. 
ASP.NET Core supports integration tests using a unit test framework with a test web host and an in-memory test server.

- A test project is used to contain and execute the tests. The test project has a reference to the SUT.
- The test project creates a test web host for the SUT and uses a test server client to handle requests and responses with the SUT.
- A test runner is used to execute the tests and report the test results.

# Customized Web application factory
ApiWeb application factory inherit from WebApplicationFactory and override ConfigureWebHost. 
The IWebHostBuilder allows the configuration of the service collection with ConfigureServices:

Database seeding in the tests is performed by the ResetDb method. The method is described in the Integration tests sample.
ApiWebApplicationFactory configured to run as "Testing" env to avoid "development" app run time config conflics like database seeding.

Mock 3rd party responses

CardCostController integration tests mock only the ​https://binlist.net/ responses.
