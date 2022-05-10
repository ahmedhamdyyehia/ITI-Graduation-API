using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{

    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
