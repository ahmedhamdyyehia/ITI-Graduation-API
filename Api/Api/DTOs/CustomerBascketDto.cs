using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class CustomerBascketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BascketItemDto> Items { get; set; }
    }
}
