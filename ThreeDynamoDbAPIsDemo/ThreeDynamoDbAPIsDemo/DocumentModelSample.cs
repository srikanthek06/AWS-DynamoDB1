using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThreeDynamoDbAPIsDemo
{
    public class DocumentModelSample
    {
        public static async Task ExecuteAsync()
        {
            using(IAmazonDynamoDB ddbClient = new AmazonDynamoDBClient())
            {
                // user V2 conversion support lists and bool types
                Table userTable = Table.LoadTable(ddbClient, "Users", DynamoDBEntryConversion.V2);

                // Save user in table
                Document newUser = new Document();
                newUser["Id"]= "ek@gmail.com";
                newUser["FirstName"]= "srkFn";
                newUser["LastName"] = "srkLn";
                newUser["Address"] = "825 OakGrove Rd";

                newUser["Active"] = false;
                newUser["NumberOfChildren"] = "3";

                newUser["Interests"] = new List<string>
                {
                   "Shoping", "Hiking", "Running", "Reading"
                };

                Document skills = new Document();
                skills["AWS-DevOps"] = 7;
                skills["DynamoDB"] = 3;
                skills["C#"] = 8;
                skills["PowerShell"] = 5;

                newUser["Skills"] = skills;

                await userTable.PutItemAsync(newUser);
                Console.WriteLine("Document User Saved");

                // Get user data
                Document loadUser = await userTable.GetItemAsync("ek@gmail.com");
                Console.WriteLine("Reading User");
                PrintDocument(loadUser);
            }
        }

        private static void PrintDocument(Document loadUser)
        {
            Console.WriteLine($"Id: {loadUser["Id"].ToString()}");
            Console.WriteLine($"First Name: {loadUser["FirstName"].ToString()}");
            Console.WriteLine($"Last Name: {loadUser["LastName"].ToString()}");
            Console.WriteLine($"Address: {loadUser["Address"].ToString()}");
            Console.WriteLine($"Active: {loadUser["Active"].AsBoolean()}");
            Console.WriteLine($"Number Of Children: {loadUser["NumberOfChildren"]}");

            Console.WriteLine("Interests: ");
            DynamoDBList interests = loadUser["Interests"] as DynamoDBList;
            foreach (var item in interests.Entries)
            {

                Console.WriteLine($"    {item}");
            }

            Console.WriteLine("Skills: ");
            var skills = loadUser["Skills"] as Document ;
            foreach (var key in skills.Keys)
            {
                var item = skills[key];
                Console.WriteLine($"    {key}: {item}");
            }
        }
    }
} 