using Api.DTOs;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Models;
using Core.Models.Identity;
using Core.Models.OrderAggregate;

namespace Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Products, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))  //  in this method MapFrom (2nd Method) we are gonna passing the source where do we want to get the property from
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            // the first thing is we gonna give it an expression
            // what we are affecting here is the destination member which is ProductToReturnDTO

            CreateMap<Core.Models.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<TypeToCreate, ProductType>();
            CreateMap<BrandToCreate, ProductBrand>();
            CreateMap<ProductCreateDto, Products>();
            CreateMap<CustomerBascketDto, CustomerBasket>();
            CreateMap<BascketItemDto, BasketItem>();
            CreateMap<AddressDto, Core.Models.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>().ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}
