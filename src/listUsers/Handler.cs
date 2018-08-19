using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class User
{
    public string id;
    public string FirstName;
    public string LastName;
    public string FavoriteColor;
}

// List users
namespace StackeryFunction
{
    public class Handler
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<dynamic> handler(APIGatewayProxyRequest message)
        {
            Console.WriteLine(message);
            var client = new AmazonDynamoDBClient();
            var tableName = Environment.GetEnvironmentVariable("TABLE_NAME");
            var table = Table.LoadTable(client, tableName);

            var search = table.Scan(new ScanOperationConfig
            {
                Select = SelectValues.AllAttributes,
                CollectResults = true
            });

            List<User> userList = new List<User>();
            do {
                foreach (var document in await search.GetNextSetAsync())
                {
                    userList.Add(new User
                    {
                        id = document["id"],
                        FirstName = document["FirstName"],
                        LastName = document["LastName"],
                        FavoriteColor = document["FavoriteColor"]
                    });
                }
            } while (!search.IsDone);

            return new APIGatewayProxyResponse{
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(userList)
            };
        }
    }
}
