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
            CreateMap<User, UserModelDetail>().ReverseMap();

            CreateMap<UserRole, UserRoleModel>().ReverseMap();

            CreateMap<RoleCreateModel, Role>();
            CreateMap<Role, RoleModel>();

            CreateMap<UomCategory, UomCategoryModel>().ReverseMap();
            CreateMap<UomCategory, UomCategoryCreate>().ReverseMap();
            CreateMap<UomCategory, UomCategoryInfo>().ReverseMap();
            CreateMap<UomUom, UomUomCollection>().ReverseMap();

            CreateMap<UomUom, UomUomModel>().ReverseMap();
            CreateMap<UomUomCreate, UomUom>()
                .ForMember(dest => dest.FactorInv, opt => opt.Ignore());
            CreateMap<UomUom, UomUomUpdateType>().ReverseMap();
            CreateMap<UomUom, UomUomUpdateFactor>().ReverseMap();

            CreateMap<ProductRemoval, ProductRemovalModel>().ReverseMap();
            CreateMap<ProductRemovalCreate, ProductRemoval>().ReverseMap();
            CreateMap<ProductRemovalUpdate, ProductRemoval>().ReverseMap();

            CreateMap<ProductCategory, ProductCategoryModel>().ReverseMap();
            CreateMap<ProductCategoryInfo, ProductCategory>().ReverseMap();
            CreateMap<ProductCategoryCreate, ProductCategory>().ReverseMap();
            CreateMap<ProductCategoryUpdate, ProductCategory>().ReverseMap();

            CreateMap<ProductAttribute, ProductAttributeModel>().ReverseMap();
            CreateMap<ProductAttribute, ProductAttributeInfo>().ReverseMap();
            CreateMap<ProductAttributeCreate, ProductAttribute>().ReverseMap();
            CreateMap<ProductAttributeUpdate, ProductAttribute>().ReverseMap();

            CreateMap<ProductAttributeValue, ProductAttributeValueModel>().ReverseMap();
            CreateMap<ProductAttributeValueCreate, ProductAttributeValue>().ReverseMap();
            CreateMap<ProductAttributeValueUpdate, ProductAttributeValue>().ReverseMap();

            CreateMap<ProductTemplateAttributeLine, ProductTemplateAttributeLineModel>().ReverseMap();
            CreateMap<ProductTemplateAttributeLineCreate, ProductTemplateAttributeLine>().ReverseMap();
            CreateMap<ProductTemplateAttributeLineUpdate, ProductTemplateAttributeLine>().ReverseMap();
            CreateMap<ProductTemplateAttributeLineInfo, ProductTemplateAttributeLine>().ReverseMap();

            CreateMap<ProductTemplate, ProductTemplateModel>().ReverseMap();
            CreateMap<ProductTemplateCreate, ProductTemplate>().ReverseMap();
            CreateMap<ProductTemplateUpdate, ProductTemplate>().ReverseMap();
            CreateMap<ProductTemplateInfo, ProductTemplate>();

            CreateMap<ProductTemplate, ProductTemplateInfo>()
                .ForMember(dest => dest.QtyAvailable, opt => opt.MapFrom(src => src.ProductProducts.SelectMany(pp => pp.StockQuants).Sum(sq => sq.Quantity)));

            CreateMap<ProductTemplateAttributeValue, ProductTemplateAttributeValueModel>().ReverseMap();

            CreateMap<ProductProductCreate, ProductProduct>().ReverseMap();

            CreateMap<ProductVariantCombinationCreate, ProductVariantCombination>().ReverseMap();

            CreateMap<StockWarehouse, StockWarehouseModel>().ReverseMap();
            CreateMap<StockWarehouse, StockWarehouseCreate>().ReverseMap();
            CreateMap<StockWarehouse, StockWarehouseUpdate>().ReverseMap();
            CreateMap<StockWarehouse, StockWarehouseInfo>().ReverseMap();

            CreateMap<StockLocation, StockLocationModel>().ReverseMap();
            CreateMap<StockLocation, StockLocationInfo>().ReverseMap();
            CreateMap<StockLocation, StockLocationCreate>().ReverseMap();

            CreateMap<StockPickingType, StockPickingTypeModel>().ReverseMap();
            CreateMap<StockPickingType, StockPickingTypeInfo>()
                .ForMember(dest => dest.TotalPickingReady, opt => opt.MapFrom(src => src.StockPickings.Where(_ => _.State == Data.Enums.PickingState.Assigned).Count()));

            CreateMap<StockQuantCreate, StockQuant>().ReverseMap();
            CreateMap<StockQuantModel, StockQuant>().ReverseMap();
            CreateMap<StockQuantInfo, StockQuant>().ReverseMap();

            CreateMap<StockPicking, StockPickingModel>().ReverseMap();
            CreateMap<StockPicking, StockPickingCreate>().ReverseMap();
            CreateMap<StockPicking, StockPickingUpdate>().ReverseMap();
            CreateMap<StockPicking, StockPickingReceipt>().ReverseMap();
            CreateMap<StockPicking, StockPickingDeliveryOrder>().ReverseMap();
            CreateMap<StockPicking, StockPickingInternalTransfer>().ReverseMap();

            CreateMap<StockMoveCreate, StockMove>().ReverseMap();
        }
    }
}
