using AutoMapper;
using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.ValueObjects;

namespace RandomApp.ShoppingCartManagement.Application.Mapping
{
    public class ShoppingCartMappingProfile : Profile
    {
        public ShoppingCartMappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<ShoppingCartDto, ShoppingCart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<ShoppingCartItem, ShoppingCartItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            CreateMap<ShoppingCartItemDto, ShoppingCartItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.ShoppingCartId, opt => opt.Ignore())  
                .ForMember(dest => dest.ShoppingCart, opt => opt.Ignore()); 
        }
    }
}