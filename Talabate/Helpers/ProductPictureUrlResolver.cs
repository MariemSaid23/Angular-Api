using AutoMapper;
using Talabat.core.Entities;
using Talabate.Dtos;

namespace Talabate.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {


        private readonly IConfiguration _configration;
        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configration["ApiBaseUrl"]}/{source.PictureUrl}";
            return string.Empty;
        } 
    }
}
