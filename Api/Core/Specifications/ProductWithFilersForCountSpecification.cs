using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductsWithFiltersForCountSpecification : BaseSpecifcation<Products>
    {
        public ProductsWithFiltersForCountSpecification(ProductSpecParams productParams) : base(x =>
           (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
           (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
           (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
        {

        }

    }
}
