using Core.Interfaces;
using Core.Models;
using Core.Models.OrderAggregate;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            BasketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }

        public IBasketRepository BasketRepository { get; }

        public  async Task<Order> CreateOrderAsync(string buyerEmail, Address shipToAddress, int deliveryMethodId, string basketId)
        {

            // get basket from repo 
            var basket =  await BasketRepository.GetBasketAsync(basketId);
            // get items from product repo
            
            var items= new List<OrderItem>();

            foreach(var item in basket.Items)
            {
                var productItem = await unitOfWork.Repository<Products>().GetByIdAsync(item.Id);
                var itemOrderd = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrderd, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            // get delivery method from repo 

            var deliveryMethod =  await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync
                (deliveryMethodId);
            //culc subtotal 
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            //create order 
            var order = new Order(items, buyerEmail, shipToAddress, deliveryMethod, subtotal);

            unitOfWork.Repository<Order>().Add(order);

            //save in db

            var result = await unitOfWork.Complete();

            if (result <= 0) return null;

            //delete basket 

            await BasketRepository.DeleteBasketAsync(basketId);
            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUSerAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}
