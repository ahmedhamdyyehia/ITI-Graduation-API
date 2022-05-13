using Core.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecifcation<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(string email):base(o=>o.BuyerEmail==email)
        {
            AddInclude(o=>o.DeliveryMethod);
            AddInclude(o=>o.OrderItems);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecification(int id ,string email) 
              : base(o=>o.Id==id && o.BuyerEmail==email)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
        }

        public OrdersWithItemsAndOrderingSpecification(int id)
              : base(o => o.Id == id)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.ShipToAddress);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecification()
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.ShipToAddress);
            AddOrderByDescending(o => o.OrderDate);
        }


    }
}
