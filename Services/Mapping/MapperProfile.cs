using AutoMapper;
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
            
            CreateMap<RoleCreateModel, Role>();
            CreateMap<Role, RoleModel>();

            CreateMap<SupplierCreateModel, Supplier>();
            CreateMap<SupplierUpdateModel, Supplier>();
            CreateMap<SupplierModel, Supplier>().ReverseMap();

            CreateMap<ReceiptCreateModel, Receipt>();
            CreateMap<ReceiptUpdateModel, Receipt>();
            CreateMap<ReceiptModel, Receipt>().ReverseMap();

            CreateMap<ProductCreateModel, Product>();
            CreateMap<ProductUpdateModel, Product>();
            CreateMap<ProductModel, Product>().ReverseMap();

            CreateMap<InboundProductCreateModel, InboundProduct>();
            CreateMap<InboundProductUpdateModel, InboundProduct>();
            CreateMap<InboundProductModel, InboundProduct>().ReverseMap();

            CreateMap<LocationCreateModel, Location>();
            CreateMap<LocationUpdateModel, Location>();
            CreateMap<LocationModel, Location>().ReverseMap();

            CreateMap<PickingRequestCreateModel, Location>();
            CreateMap<PickingRequestUpdateModel, Location>();
            CreateMap<PickingRequestModel, PickingRequest>().ReverseMap();
        }
    }
}
