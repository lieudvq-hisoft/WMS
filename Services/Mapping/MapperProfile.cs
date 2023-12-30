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
            CreateMap<User, ProfileModel>();

            CreateMap<RoleCreateModel, Role>();
            CreateMap<Role, RoleModel>();

            CreateMap<SupplierCreateModel, Supplier>();
            CreateMap<SupplierUpdateModel, Supplier>();
            CreateMap<SupplierModel, Supplier>().ReverseMap();

            CreateMap<ReceiptCreateModel, Receipt>();
            CreateMap<ReceiptUpdateModel, Receipt>();
            CreateMap<ReceiptModel, Receipt>().ReverseMap();
            CreateMap<Receipt, ReceiptReport>().ReverseMap();
            CreateMap<ReceiptCompletedModel, Receipt>().ReverseMap();


            CreateMap<ProductCreateModel, Product>();
            CreateMap<ProductUpdateModel, Product>();
            CreateMap<ProductModel, Product>().ReverseMap();
            CreateMap<ProductCompletedModel, Product>().ReverseMap();


            CreateMap<LocationCreateModel, Location>();
            CreateMap<LocationUpdateModel, Location>();
            CreateMap<LocationModel, Location>().ReverseMap();
            CreateMap<InventoryLocation, InventoryLocationModel>();
            CreateMap<InventoryLocation, InventoryLocationFPModel>().ReverseMap();
            CreateMap<InventoryLocation, InventoryLocationFIModel>().ReverseMap();
            CreateMap<LocationFPModel, Location>().ReverseMap();
            CreateMap<LocationFIModel, Location>().ReverseMap();

            CreateMap<PickingRequestCreateModel, PickingRequest>();
            CreateMap<PickingRequestUpdateModel, PickingRequest>();
            CreateMap<PickingRequestModel, PickingRequest>().ReverseMap();
            CreateMap<PickingRequestCompletedModel, PickingRequest>().ReverseMap();


            CreateMap<Inventory, InventoryModel>();
            CreateMap<Inventory, InventoryFIModel>();
            CreateMap<InventoryLocation, InventoryLocationPrivateModel>();
            CreateMap<Inventory, InventoryPrivateModel>();
            CreateMap<Inventory, InventoryFPModel>().ReverseMap();

            CreateMap<RackCreateModel, Rack>();
            CreateMap<RackUpdateModel, Rack>();
            CreateMap<RackModel, Rack>().ReverseMap();

            CreateMap<RackLevelCreateModel, RackLevel>();
            CreateMap<RackLevelUpdateModel, RackLevel>();
            CreateMap<RackLevelModel, RackLevel>().ReverseMap();
        }
    }
}
