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

        public async Task<int> AddEntityAsync(T Entity)
        {
            await context.Set<T>().AddAsync(Entity);
            return context.SaveChanges();          
        }

        public async Task<int> UpdateEntityAsync(T Entity)
        {
            context.Entry(Entity).State = EntityState.Modified;
            return await context.SaveChangesAsync();           
        }

        public async Task<int> DeleteEntityAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {                  
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

        public async Task<List<Products>> GetLatestAddedProductsAsync()
        {
            return await context.Products.OrderByDescending(x=>x.Id).Take(8).ToListAsync();
        }

        private IQueryable<T> ApplySpecification (ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(),spec);
        }

        public void Add(T entity)
        {
            
            context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            context.Set<T>().Attach(entity);
            context.Entry(entity).State=EntityState.Modified;
        }

        public void Delete(T entity)
        {

            context.Set<T>().Remove(entity);
        }
    }
}
