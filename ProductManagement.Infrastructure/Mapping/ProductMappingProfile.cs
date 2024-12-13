using AutoMapper;
using Random.App.ProductManagement.Domain.Entities;
using Random.App.ProductManagement.Infrastructure.Data_Transfer_Objects;

namespace Random.App.ProductManagement.Infrastructure.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Name,
                           opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.Id));
        }
    }
}
