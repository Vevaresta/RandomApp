using AutoMapper;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Domain.Entities;

namespace RandomApp.ProductManagement.Application.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalApiId, opt => opt.MapFrom(src => src.Id))
                // if Title is null or empty return "Unknown Product" else Title
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Title) ? "Unknown Product" : src.Title))
                // if Category is null or empty return "Uncategorized" else Category
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Category) ? "Uncategorized" : src.Category));
        }
    }
}
