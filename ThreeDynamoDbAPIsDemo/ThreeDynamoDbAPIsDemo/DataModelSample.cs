using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThreeDynamoDbAPIsDemo
{
    public class DataModelSample
    {
        public static async Task ExecuteAsync()
        {
            DynamoDBContextConfig config = new DynamoDBContextConfig
            {
                ConsistentRead = true,
                Conversion = DynamoDBEntryConversion.V2
            };

            using (DynamoDBContext context = new DynamoDBContext(new AmazonDynamoDBClient(), config))
            {
                User newUser = new User
                {
                    Email = "test@gmail.com",
                    FirstName = "TestFn",
                    LastName = "LnTest",
                    Address = "Seattle, WA",
                    Active = true,
                    NumberOfChildren = 6,
                    Interests = new List<string>
                    {
                        "TV","Movie","Coding","Walking","Driving"
                    },
                    Skills= new Dictionary<string, int>
                    {
                        {"C#",9 },
                        {".NetCore", 8 },
                        {"EFCore", 5 }
                    }
                };

                await context.SaveAsync(newUser);
                Console.WriteLine("User Saved");

                User loadUser = await context.LoadAsync<User>("test@gmail.com");

                Console.WriteLine("Reading User");
                PrintUser(loadUser);
            }
        }

        private static void PrintUser(User loadUser)
        {
            Console.WriteLine($"First Name: {loadUser.FirstName}");
            Console.WriteLine($"Last Name: {loadUser.LastName}");
            Console.WriteLine($"Address: {loadUser.Address}");
            Console.WriteLine($"Active: {loadUser.Active}");
            Console.WriteLine($"Number Of Children: {loadUser.NumberOfChildren}");

            Console.WriteLine("Interest:");
            foreach (var interest in loadUser.Interests)
            {
                Console.WriteLine($"    {interest}");
            }

            Console.WriteLine($"Skills:");
            var skills = loadUser.Skills;
            foreach (var key in skills.Keys)
            {
                Console.WriteLine($"    {key}: {skills[key]}");
            }
        }
    }


    [DynamoDBTable("Users")]
    public class User
    {
        [DynamoDBProperty("Id")]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool Active { get; set; }
        public int NumberOfChildren { get; set; }
        public List<string> Interests { get; set; }
        public Dictionary<string,int> Skills { get; set; }
        
        [DynamoDBIgnore]
        public string FullName { get { return FirstName + ", " + LastName; } }

    }
}