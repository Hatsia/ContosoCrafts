using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName => Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json");

        public IEnumerable<Product> GetProducts()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);
            return JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public void AddRating(string productID, int rating)
        {
            var products = GetProducts();
            var query = products.First(x => x.Id == productID);
            if (query.Ratings == null)
            {
                query.Ratings = new int[] { rating };
            }
            else
            {
                var ratings = query.Ratings.ToList();
                ratings.Add(rating);
                query.Ratings = ratings.ToArray();

            }

            //Write edited(addded rating) Product to json file.
            /* Mistake discription and it resolve:
            FROM Marvel SE. Youtube comment.
                For anyone who notices the error at 15:35:

                1. Go to JsonFileProductService.cs
                2. Go to the AddRating method
                3. Change File.OpenWrite(JsonFileName) to File.Open(JsonFileName, FileMode.Truncate)

                The reason this was causing an error is because the length of the string being written was different than what the product.json file had. If you read the microsoft docs, the remarks say a difference in length will cause a sort of 'mix' between the two. 
            */
            using (var outputStream = File.Open(JsonFileName, FileMode.Truncate)) //It was File.OpenWrite(JsonFileName) - and it caused a mistake in product.json, when the url was inputed in browser.
            {
                JsonSerializer.Serialize<IEnumerable<Product>>( //It's not a only one way to write some text. It's used to write if a user speaks not only English, maybe Chinese or Japanese.
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions {
                        SkipValidation = true,
                        Indented = false
                    }),
                    products
                );
            }
        }

    }
}