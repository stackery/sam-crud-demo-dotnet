using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;

// Delete user
namespace StackeryFunction
{
    public class Handler
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<dynamic> handler(APIGatewayProxyRequest message)
        {
            Console.WriteLine(message);
            string id = message.PathParameters["id"];

            var client = new AmazonDynamoDBClient();
            var tableName = Environment.GetEnvironmentVariable("TABLE_NAME");
            var table = Table.LoadTable(client, tableName);

            try
            {
                var result = await table.DeleteItemAsync(id);
                Console.WriteLine("result", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeleteItem failed: " + ex.Message);
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = 204
            };
        }
    }
}