using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository : IProdcutRepository
    {
        private readonly WebDbContext context;

        public ProductRepository(WebDbContext _context)
        {
            context = _context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync()
        {
            return await context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<Products>> GetAllProductsAsync()
        {
            return await context.Products.
                Include(p=>p.ProductBrand)
                .Include(p=>p.ProductType)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync()
        {
            return await context.ProductTypes.ToListAsync();
        }

        public async Task<Products> GetProductByIdAsync(int id)
        {
            return await context.Products
                .Include(p=>p.ProductBrand)
                .Include(p=>p.ProductType)
                .FirstOrDefaultAsync(); 

            // we use here FirstOrDefault Instead of FindAsync() // Becuase Find does not accept IQuerable .
                
        }
    }
}
