using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class TypeToCreate
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }       
        public string PictureUrl  { get; set; }
    }
}
