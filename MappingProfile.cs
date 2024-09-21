using authmodule.Entitis;
using authmodule.Models;
using AutoMapper;

namespace authmodule
{
    public class MappingProfile : Profile
    {

        public MappingProfile() 
        {
            CreateMap<Users, LoginResponse>().ReverseMap();
        }  
    }
}