﻿using AutoMapper;
using Data.Entities;
using Data.Model;
using Data.Models;

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

            CreateMap<UomCategory, UomCategoryModel>().ReverseMap();
            CreateMap<UomCategory, UomCategoryCreate>().ReverseMap();
            CreateMap<UomUom, UomUomCollection>().ReverseMap();

            CreateMap<UomUom, UomUomModel>().ReverseMap();
            CreateMap<UomUom, UomUomCreate>().ReverseMap();
            CreateMap<UomUom, UomUomUpdateType>().ReverseMap();
            CreateMap<UomUom, UomUomUpdateFactor>().ReverseMap();
        }
    }
}
