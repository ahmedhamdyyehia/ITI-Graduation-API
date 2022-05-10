using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.OrderAggregate
{
    public class ProductItemOrdered
    {

        public ProductItemOrdered()
        { 
        }
            public ProductItemOrdered(int productItemId, string productName,string pictureUrl)
        {
            ProductItemId=productItemId;
            ProductName = productName;
            PictureUrl=pictureUrl;

        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }

        public string PictureUrl { get; set; }
    }
}
