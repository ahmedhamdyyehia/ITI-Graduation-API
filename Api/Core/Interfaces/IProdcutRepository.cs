using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProdcutRepository
    {
        Task<Products> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Products>> GetAllProductsAsync();
        Task<IReadOnlyList<ProductType>> GetAllProductTypesAsync();
        Task<IReadOnlyList<ProductBrand>> GetAllProductBrandsAsync();
    }
}
