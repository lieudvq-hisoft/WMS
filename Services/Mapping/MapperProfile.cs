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
            CreateMap<User, UserModel>();
            CreateMap<User, ProfileModel>();
            CreateMap<User, UserModelDetail>().ReverseMap();

            CreateMap<UserRole, UserRoleModel>().ReverseMap();

            CreateMap<RoleCreateModel, Role>();
            CreateMap<Role, RoleModel>();
        }
    }
}
