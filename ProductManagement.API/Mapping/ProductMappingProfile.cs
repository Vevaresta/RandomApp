using AutoMapper;
using NLog.Web.LayoutRenderers;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Domain.Enums;
using RandomApp.ProductManagement.Domain.ValueObjects;

namespace RandomApp.ProductManagement.Application.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OriginalApiId, opt => opt.MapFrom(src => src.OriginalApiId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Price(src.Amount, src.Currency ?? "USD")))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => SKU.Create(src.SKU)))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<Category>(src.Category)))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => new ProductDescription(src.ProductDescription)))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));


            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OriginalApiId, opt => opt.MapFrom(src => src.OriginalApiId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU.Value))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.ProductDescription.Value))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

        }
    }
}
