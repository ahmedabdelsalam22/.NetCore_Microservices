using AutoMapper;
using Mango.Web.Models;
using Mango.Web.Models.DTOS;

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
