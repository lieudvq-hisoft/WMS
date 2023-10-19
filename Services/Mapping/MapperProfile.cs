using AutoMapper;
using Data.Entities;
using Data.Model;

namespace Services.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserCreateModel, User>();
            
            CreateMap<RoleCreateModel, Role>();
        }
    }
}
