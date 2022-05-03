using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
            
        }
        public CustomerBasket(string id)
        {
            Id = id;
        }
        //because the  basket is our customer thing and not something we are storing in DB
        //so we will let the customer generate the id of the basket , i mean by that our
        //angular application will generate Id. we will use a unique identifier for each
        //busket we create
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
