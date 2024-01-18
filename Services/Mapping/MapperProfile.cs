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
            CreateMap<ProductModel, Product>();
            CreateMap<Product, ProductModel>()
                .ForMember(dest => dest.TotalInventory,
                   opt => opt.MapFrom(src => src.Inventories.Where(_ => _.IsAvailable).Count()));
            CreateMap<ProductCompletedModel, Product>().ReverseMap();


            CreateMap<LocationCreateModel, Location>();
            CreateMap<LocationUpdateModel, Location>();
            CreateMap<LocationModel, Location>().ReverseMap();
            CreateMap<InventoryLocation, InventoryLocationModel>();
            CreateMap<InventoryLocation, InventoryLocationInnerModel>();
            CreateMap<InventoryLocation, InventoryLocationFPModel>().ReverseMap();
            CreateMap<InventoryLocation, InventoryLocationFIModel>().ReverseMap();
            CreateMap<LocationFPModel, Location>().ReverseMap();
            CreateMap<LocationFIModel, Location>().ReverseMap();
            CreateMap<LocationInnerModel, Location>().ReverseMap();

            CreateMap<PickingRequestCreateModel, PickingRequest>();
            CreateMap<PickingRequestInnerCreateModel, PickingRequest>();
            CreateMap<PickingRequestUpdateModel, PickingRequest>();
            CreateMap<PickingRequestModel, PickingRequest>().ReverseMap();
            CreateMap<PickingRequestDetailModel, PickingRequest>().ReverseMap();
            CreateMap<PickingRequestCompletedModel, PickingRequest>().ReverseMap();
            CreateMap<PickingRequestInnerOrderModel, PickingRequest>().ReverseMap();
            CreateMap<PickingRequestUserInnerModel, PickingRequestUser>().ReverseMap();
            CreateMap<PickingRequestInventoryInner, Data.Entities.PickingRequestInventory>().ReverseMap();

            CreateMap<Inventory, InventoryModel>();
            CreateMap<Inventory, InventoryFIModel>();
            CreateMap<InventoryLocation, InventoryLocationPrivateModel>();
            CreateMap<Inventory, InventoryPrivateModel>();
            CreateMap<Inventory, InventoryFPModel>().ReverseMap();
            CreateMap<Inventory, InventoryInnerModel>().ReverseMap();

            CreateMap<RackCreateModel, Rack>();
            CreateMap<RackUpdateModel, Rack>();
            CreateMap<RackModel, Rack>().ReverseMap();

            CreateMap<RackLevelCreateModel, RackLevel>();
            CreateMap<RackLevelUpdateModel, RackLevel>();
            CreateMap<RackLevelModel, RackLevel>().ReverseMap();

            CreateMap<Order, OrderInnerModel>().ReverseMap();
            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<Order, OrderCreateModel>().ReverseMap();

            CreateMap<InventoryThreshold, InventoryThresholdModel>().ReverseMap();
            CreateMap<InventoryThreshold, InventoryThresholdCreateModel>().ReverseMap();

        }
    }
}
