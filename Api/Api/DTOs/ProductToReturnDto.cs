namespace Api.DTOs
{

    // DTO stands for Data transfer object
    // DTO does not contain any business logic , they only have simple setters and getters
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }    
        public string PictureUrl { get; set; }  
      
        public string ProductType { get; set; }
       
        public string ProductBrand { get; set; }
    }
}
