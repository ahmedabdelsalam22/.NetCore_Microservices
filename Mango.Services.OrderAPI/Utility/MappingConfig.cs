using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dtos;

namespace Mango.Services.OrderAPI.Utility
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {

            CreateMap<OrderHeader,OrderHeaderDto>().ReverseMap();
            CreateMap<OrderDetailsDto,OrderDetails>().ReverseMap();

            CreateMap<OrderHeaderDto,CartHeaderDto>()
                .ForMember(dest=>dest.CartTotal, x=>x.MapFrom(src=>src.OrderTotal)).ReverseMap();

            CreateMap<CartDetailsDto, OrderDetailsDto>()
               .ForMember(dest => dest.ProductName, x => x.MapFrom(src => src.Product.Name))
               .ForMember(dest => dest.Price, x => x.MapFrom(src => src.Product.Price));

            CreateMap<OrderDetailsDto, CartDetailsDto>();

        }
    }
}
