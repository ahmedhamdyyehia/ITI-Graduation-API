using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using System.Reflection;

namespace Infrastructure.Data
{
    public class WebDbContext : DbContext
    {
    

        public WebDbContext (DbContextOptions<WebDbContext> options) : base(options)
        {
            
        }
        public  DbSet<Products> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }


       

    }
}
