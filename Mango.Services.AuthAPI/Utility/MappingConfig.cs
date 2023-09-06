using AutoMapper;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTOS;

namespace Mango.Services.AuthAPI.Utility
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser,UserDTO>();
        }
    }
}
