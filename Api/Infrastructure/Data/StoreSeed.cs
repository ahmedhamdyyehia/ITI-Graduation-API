using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreSeed
    {
        public static async Task SeedDataAsync(WebDbContext context)
        {
            if (!context.ProductBrands.Any())
            {
                // reading from json file
                var brandsData =
                    File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                //deserilzeing the data to C# list
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                // adding the data to database
                foreach (var item in brands)
                {
                    context.ProductBrands.Add(item);
                }

                await context.SaveChangesAsync();
            }
            if (!context.ProductTypes.Any())
            {
                // reading from json file
                var typesData =
                File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                //deserilzeing the data to C# list
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                // adding the data to database
                foreach (var item in types)
                {
                    context.ProductTypes.Add(item);
                }

                await context.SaveChangesAsync();
            }
            if (!context.Products.Any())
            {
                // reading from json file
                var productsData =
                File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                //deserilzeing the data to C# list
                var products = JsonSerializer.Deserialize<List<Products>>(productsData);
                // adding the data to database
                foreach (var item in products)
                {
                    context.Products.Add(item);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
