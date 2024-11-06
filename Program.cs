using System;
using System.Threading.Tasks;
// NuGet packages
using Microsoft.PowerPlatform.Dataverse.Client; // For Dataverse interaction
using Microsoft.Extensions.Configuration; // For configuration management
using Microsoft.Xrm.Sdk; // For CRM entities and operations
using Microsoft.Xrm.Sdk.Query;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;


namespace CRM
{
    class Program
    {
        static async Task Main(string[] args)
        {


            const string clientSecret = ""; // Application (Client) ID
            const string clientId = ""; // Client Secret
            const string dynamicsUrl = ""; // Your Dynamics 365 URL


            // Define connection string
            const string connectionString = $"""
                                             
                                                             AuthType=ClientSecret;
                                                             Url={dynamicsUrl};
                                                             ClientId={clientId};
                                                             ClientSecret={clientSecret};
                                                             LoginPrompt=Never;
                                                         
                                             """;
            
            Console.WriteLine($"Connection string is :-{connectionString}");

            try
            {
                // Create a new instance of ServiceClient using the connection string
                Console.WriteLine("Connecting to Dynamics 365...");
                using var serviceClient = new ServiceClient(connectionString);

                if (serviceClient.IsReady)
                {
                    Console.WriteLine("Connected to Dynamics 365 successfully.");

                    // Retrieve contacts using a QueryExpression
                    Console.WriteLine("Preparing to retrieve contacts...");
                    var query = new QueryExpression("contact")
                    {
                        ColumnSet = new ColumnSet("fullname", "emailaddress1", "telephone1"),
                        Orders =
                        {
                            new OrderExpression("fullname", OrderType.Ascending)
                        },
                        
                    };
                    
                    // var query = new QueryExpression("contact")
                    // {
                    //     ColumnSet = new ColumnSet(true), // Retrieve all columns
                    //     PageInfo = new PagingInfo()
                    //     {
                    //         PageNumber = 1,
                    //         Count = 5000
                    //     }
                    // };
                    


                    Console.WriteLine("Retrieving contacts...");

                    // Execute the query
                    Console.WriteLine("Executing query... ");
                    var contacts = await serviceClient.RetrieveMultipleAsync(query);
                    Console.WriteLine("Total contacts retrieved: " + contacts.Entities.Count);


                    // Display the results
                    foreach (var entity in contacts.Entities)
                    {
                        Console.WriteLine($"Name: {entity.GetAttributeValue<string>("fullname")}");
                        Console.WriteLine($"Email: {entity.GetAttributeValue<string>("emailaddress1")}");
                        Console.WriteLine($"Phone: {entity.GetAttributeValue<string>("telephone1")}");
                        Console.WriteLine(new string('-', 50));
                    }
                }
                else
                {
                    Console.WriteLine("Failed to connect to Dynamics 365.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
