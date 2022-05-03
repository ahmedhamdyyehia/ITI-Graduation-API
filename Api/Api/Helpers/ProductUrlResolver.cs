using Api.DTOs;
using AutoMapper;
using Core.Models;
using Microsoft.Extensions.Configuration;

namespace Api.Helpers
{

    // now we are going to specify the types
    // the source (products) Where we wanna map from and where we wanna map To (ProductToReturnDto)
    // then where we want the destination property to be. => string because we want to return picture URl
    public class ProductUrlResolver : IValueResolver<Products, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config; // which the api will need for this part

        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Products source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            // first we want to check if the picture is empty or not.
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}
