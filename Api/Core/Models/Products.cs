using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Core.Models
{
    public class Products : BaseEntity
    {
        [Required, MaxLength(500)]
        public string Name { get; set; }
        [Required, MaxLength(1500)]

        public string Description { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [ForeignKey("ProductType")]
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
        [ForeignKey("ProductBrand")]
        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }

    }
}
