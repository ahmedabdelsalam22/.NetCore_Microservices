using AutoMapper;
using Mango.Web.Models;
using Mango.Web.Models.Dtos;

namespace Mango.Web.Utility
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
