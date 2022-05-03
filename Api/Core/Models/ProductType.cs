using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class ProductType : BaseEntity
    {
        [Required,MaxLength(100)]
        public string Name { get; set; }
        public virtual List<Products> Products { get; set; }

    }
}
