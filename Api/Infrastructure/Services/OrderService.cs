using Core.Interfaces;
using Core.Models;
using Core.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            BasketRepository = basketRepository;
            UnitOfWork = unitOfWork;
        }

        public IBasketRepository BasketRepository { get; }
        public IUnitOfWork UnitOfWork { get; }

        public  async Task<Order> CreateOrderAsync(string buyerEmail, Address shipToAddress, int deliveryMethodId, string basketId)
        {

            // get basket from repo 
            var basket =  await BasketRepository.GetBasketAsync(basketId);
            // get items from product repo
            
            var items= new List<OrderItem>();

            foreach(var item in basket.Items)
            {
                var productItem = await UnitOfWork.Repository<Products>().GetByIdAsync(item.Id);
                var itemOrderd = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrderd, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            // get delivery method from repo 

            var deliveryMethod =  await UnitOfWork.Repository<DeliveryMethod>().GetByIdAsync
                (deliveryMethodId);
            //culc subtotal 
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            //create order 
            var order = new Order(items, buyerEmail, shipToAddress, deliveryMethod, subtotal);

            UnitOfWork.Repository<Order>().Add(order);

            //save in db

            var result = await UnitOfWork.Complete();

            if (result <= 0) return null;

            //delete basket 

            BasketRepository.DeleteBasketAsync(basketId);
            return order;

        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersByUSerAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
