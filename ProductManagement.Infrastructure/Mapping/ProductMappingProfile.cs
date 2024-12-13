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
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalApiId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Title) ? "Unknown Product" : src.Title))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Category) ? "Uncategorized" : src.Category));
        }
    }
}
