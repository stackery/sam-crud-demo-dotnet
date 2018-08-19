using System;
using System.Json;
using System.Threading.Tasks;  
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Runtime;

// Create user
namespace StackeryFunction
{
    public class Handler
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<dynamic> handler(APIGatewayProxyRequest message)
        {
            Console.WriteLine("Here in POST function");
            Console.WriteLine(message);

            string body = message.Body;
            Console.WriteLine("Body: " + body);
            JsonValue value = JsonValue.Parse(body);
            JsonObject userObj = value as JsonObject;

            Console.WriteLine("userObj", userObj);

            var doc = new Document();
            doc["id"] = (string)userObj["id"];
            doc["FirstName"] = (string)userObj["firstName"];
            doc["LastName"] = (string)userObj["lastName"];
            doc["FavoriteColor"] = (string)userObj["color"];

            var client = new AmazonDynamoDBClient();
            var tableName = Environment.GetEnvironmentVariable("TABLE_NAME");
            var table = Table.LoadTable(client, tableName);

            try
            {
                var result = await table.PutItemAsync(doc);
                Console.WriteLine("result", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Table.Put failed: " + ex.Message);
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = 204
            };
        }
    }
}