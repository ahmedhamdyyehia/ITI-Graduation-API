using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // criteria is like .Where()
        Expression<Func<T,bool>> Criteria { get; }


        // this is for include operations.
        // Include is the Include() we wanna include that we are going to pass to Methods

        /*
          Include(p=>p.ProductBrand)
         .Include(p=>p.ProductType)
        */
        List<Expression<Func<T,object>>> Includes { get; }
        Expression<Func<T,object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        int Take { get; }   
        int Skip { get; }   
        bool IsPagingEnabled { get; }   


    }
}
