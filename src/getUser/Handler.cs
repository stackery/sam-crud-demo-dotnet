using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Collections.Generic;

// Get user
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

            Document userItem = await table.GetItemAsync(id);
            var response = new APIGatewayProxyResponse();
            if (userItem == null)
            {
                response.StatusCode = 404;
                return response;
            }
            response.StatusCode = 200;
            response.Body = userItem.ToJson();
            response.Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            return response;
        }
    }
}