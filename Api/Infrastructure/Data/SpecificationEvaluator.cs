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
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
        
    {
                                                        // Which is A DbSet of <Product> for ex
        public static IQueryable<TEntity> GetQuery (IQueryable<TEntity> inputQuery,
                                                    ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            // we want to evaluate what is inside ( spec ) 
            if(spec.Criteria != null)
            {
                query = query.Where(spec.Criteria); // for ex p=p.ProductId == id
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy); 
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending); 
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);  
            }

            // add the includes to the query (Criteria)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;

            // what this is gonna do?

            /*
              Include(p=>p.ProductBrand)
                .Include(p=>p.ProductType)
            */

            // this will take those 2 include statements and then aggregate them and pass them into the Query(IQuerable) that gonna pass to our list
            // or a method that can query the database and return the result basen on what is contained in those  Querable.
        }
    }
}
