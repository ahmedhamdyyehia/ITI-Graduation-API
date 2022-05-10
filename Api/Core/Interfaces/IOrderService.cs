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
        Task<Order> CreateOrderAsync(string buyerEmail ,Address shipToAddress, int deliveryMethod,String basketId);
        Task<IReadOnlyList<Order>> GetOrdersByUSerAsync(String buyerEmail);
        Task<Order> GetOrderByIdAsync(int id,string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
