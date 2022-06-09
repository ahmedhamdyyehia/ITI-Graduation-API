using Core.Models;
using Core.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public  interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail ,Address shipToAddress, int deliveryMethod,string basketId);
        Task<IReadOnlyList<Order>> GetOrdersByUSerAsync(string buyerEmail);
        Task<Order> GetOrderByIdAsync(int id,string buyerEmail);
        Task<Order> GetOrderByIdAsync(int id);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
        Task<IReadOnlyList<Order>> GetAllOrdersAsync();

        Task<bool> UpdateOrderStatus(int id);

        Task<List<OrderStatistics>> GetOrderStatistics();
        Task<List<BrandStatistics>> GetBrandsStatistics();
    }
}
