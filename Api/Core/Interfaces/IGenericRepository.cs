﻿using Core.Models;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Specifications;

namespace Core.Interfaces
{
    // <T> as a constrain so that we can use certain types with particular generic Repository
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id); // to get individual item
        Task<IReadOnlyList<T>> ListAllAsync(); // to get all items

        Task<int> AddEntityAsync(T entity);

        Task<int> UpdateEntityAsync(T entity);

        Task<int> DeleteEntityAsync(T entity);

        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<int> CountAsync(ISpecification<T> spec);

        Task<List<Products>> GetLatestAddedProductsAsync(int numberOfProducts);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<List<OrderStatistics>> GetOrderStatistics();
        Task<List<BrandStatistics>> GetBrandsStatistics();
    }
}
