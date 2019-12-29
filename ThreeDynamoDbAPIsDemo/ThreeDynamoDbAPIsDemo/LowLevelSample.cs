using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThreeDynamoDbAPIsDemo
{
    public class LowLevelSample
    {
        public static async Task ExecuteAsync()
        {
            using(IAmazonDynamoDB ddbClient = new AmazonDynamoDBClient())
            {
                // save user in table.
                await ddbClient.PutItemAsync(new PutItemRequest
                {
                    TableName = "Users",
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "Id", new AttributeValue{S="srk@gmail.com"} },
                        {"FirstName", new AttributeValue{S="Srikanth"} },
                        {"LastName", new AttributeValue{S="Krishnamurthy"} },
                        {"Address", new AttributeValue{S="1401 P ST"} },
                        {"Active", new AttributeValue{BOOL=true} },
                        {"NumberOfChildren", new AttributeValue{N="2"} },

                        {"Interests", new AttributeValue
                        {
                            L=new List<AttributeValue>
                            {
                                new AttributeValue{S="Hiking" },
                                new AttributeValue{S="Running" },
                                new AttributeValue{S="Learning New Tech" },
                                new AttributeValue{S="Video Games" }
                            }
                        } },

                        {"Skills", new AttributeValue
                        {
                            M= new Dictionary<string, AttributeValue>
                            {
                                {"C#", new AttributeValue{N="7"} },
                                {"PowerShell", new AttributeValue{N="5"} },
                                {"F#", new AttributeValue{N="2"} },
                                {"Java", new AttributeValue{N="3"} },
                                {"ReactJs", new AttributeValue{N="1"} }
                            }
                        } }
                    }
                }) ;

                Console.WriteLine("User Saved");

                // Get user back from table
                Dictionary<string, AttributeValue> item = (await ddbClient.GetItemAsync(new GetItemRequest
                {
                    TableName = "Users",
                    ConsistentRead = true,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "Id", new AttributeValue{S="srk@gmail.com"} }
                    }
                })).Item;
                Console.WriteLine("Reading User");
                PrintItem(item);

                // Delete User
                
            }
        }

        private static void PrintItem(Dictionary<string, AttributeValue> item)
        {
            Console.WriteLine($"Id: {item["Id"].S}");
            Console.WriteLine($"First Name: {item["FirstName"].S}");
            Console.WriteLine($"Last Name: {item["LastName"].S}");
            Console.WriteLine($"Address: {item["Address"].S}");
            Console.WriteLine($"Active: {item["Active"].BOOL}");
            Console.WriteLine($"Number Of Children: {item["NumberOfChildren"].N}");

            Console.WriteLine("Interests:");
            var interests = item["Interests"].L;
            foreach (var interest in interests)
            {
                Console.WriteLine($"    {interest.S}");
            }

            Console.WriteLine("Skills:");
            var skills = item["Skills"];
            foreach (var key in skills.M.Keys)
            {
                Console.WriteLine($"    {key}: {skills.M[key].N}");
            }
        }
    }
}