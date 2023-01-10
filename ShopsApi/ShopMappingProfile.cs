using AutoMapper;
using ShopsApi.Entities;
using ShopsApi.Models;

namespace ShopsApi
{
    public class ShopMappingProfile : Profile
    {
        public ShopMappingProfile()
        {
            CreateMap<Shop, Adress>();
            CreateMap<Shop, ShopDto>()
                .ForMember(x => x.City, y => y.MapFrom(z => z.Adress.City))
                .ForMember(x => x.Street, y => y.MapFrom(z => z.Adress.Street))
                .ForMember(x => x.PostalCode, y => y.MapFrom(z => z.Adress.PostalCode));

            CreateMap<Product, ProductDto>();
           
            CreateMap<CreateShopDto, Shop>()
                .ForMember(x => x.Adress, y => y.MapFrom(dto => new Adress(){City = dto.City, Street = dto.Street, PostalCode = dto.PostalCode }));

            CreateMap<CreateProductDto, Product>();
        }

        

    }
}
