using System;
using System.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;

// Update user
namespace StackeryFunction
{
    public class Handler
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<dynamic> handler(APIGatewayProxyRequest message)
        {
            Console.WriteLine(message);
            string id = message.PathParameters["id"];

            string body = message.Body;
            JsonValue value = JsonValue.Parse(body);
            JsonObject userObj = value as JsonObject;

            var doc = new Document();
            doc["id"] = id;
            doc["FirstName"] = (string)userObj["firstName"];
            doc["LastName"] = (string)userObj["lastName"];
            doc["FavoriteColor"] = (string)userObj["color"];

            var client = new AmazonDynamoDBClient();
            var tableName = Environment.GetEnvironmentVariable("TABLE_NAME");
            var table = Table.LoadTable(client, tableName);

            var result = await table.PutItemAsync(doc);

            return new APIGatewayProxyResponse{
                StatusCode = 204
            };
        }
    }
}