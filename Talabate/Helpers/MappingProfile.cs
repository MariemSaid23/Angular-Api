using AutoMapper;
using Microsoft.Extensions.Configuration;
using Talabat.core.Entities;
using Talabate.Dtos;

namespace Talabate.Helpers
{
    public class MappingProfile:Profile
    {
       // private readonly IConfiguration _configuration;
        public MappingProfile()
        {
            // _configuration = configuration;

            CreateMap<Product, ProductToReturnDto>()

                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))

                //.ForMember(d => d.PictureUrl, o => o.MapFrom(s => $"{"http://localhost:5022"}/{s.PictureUrl}"));

                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());


        }



    }
}
