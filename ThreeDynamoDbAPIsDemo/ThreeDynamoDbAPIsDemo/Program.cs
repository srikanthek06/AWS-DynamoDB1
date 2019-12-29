using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace ThreeDynamoDbAPIsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Low Level Sample");
            //LowLevelSample.ExecuteAsync().Wait();
            //Console.WriteLine("\n \n");

            //Console.WriteLine("Document Model Sample");
            //DocumentModelSample.ExecuteAsync().Wait();
            //Console.WriteLine("\n\n");

            //Console.WriteLine("Data Model Sample");
            //DataModelSample.ExecuteAsync().Wait();
            //Console.WriteLine("\n\n");

            var json = "ThreeDynamoDbAPIsDemo.entertainment.json";
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(json))
            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader reader = new JsonTextReader(streamReader))
            {
                reader.SupportMultipleContent = true;
                var serializer = new JsonSerializer();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        dynamic dynJson = serializer.Deserialize(reader);
                        Console.WriteLine("{0} {1} {2} {3}\n", dynJson.id, dynJson.displayName,
                            dynJson.slug, dynJson.imageUrl);
                    }
                }
                //dynamic dynJson = JsonConvert.DeserializeObject(json);
                //    foreach (var item in dynJson)
                //    {
                //        Console.WriteLine("{0} {1} {2} {3}\n", item.id, item.displayName,
                //            item.slug, item.imageUrl);
                //    }

            }
        }
    }
}
