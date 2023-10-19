using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mango.Services.ProductAPI.Models.Dtos
{
    public class ProductUpdateDto
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
