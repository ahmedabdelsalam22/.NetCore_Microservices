using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models.Dtos
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, 1000)]
        public double Price { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
