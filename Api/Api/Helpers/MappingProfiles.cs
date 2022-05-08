using Api.DTOs;
using AutoMapper;
using Core.Models;
using Core.Models.Identity;

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

            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
