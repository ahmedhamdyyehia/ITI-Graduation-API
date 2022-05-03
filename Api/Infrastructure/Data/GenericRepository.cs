using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity // we need to add this to our service 
                                                                                   // container so we can injectless whenever
                                                                                   // needed in our application
    {
        private readonly WebDbContext context;

        public GenericRepository(WebDbContext _context)
        {
            context = _context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            // we use set method => it create db set that can be used to query and save instances 
            // of the type of entity for which a set should be returned

            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            // here we cant use include() to access the navigation property
            // thats why we are going to use Specification pattern.

            /*return await context.Products.
             Include(p=>p.ProductBrand)
            .Include(p=>p.ProductType)
            .ToListAsync(); */
            
            // that is what we want

            return await context.Set<T>().ToListAsync();
}

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();

        }
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync(); // to get the number of results
        }

        private IQueryable<T> ApplySpecification (ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(),spec);
        }

        
    }
}
