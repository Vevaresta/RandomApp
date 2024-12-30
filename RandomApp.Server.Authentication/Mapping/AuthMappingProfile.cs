using AutoMapper;
using RandomApp.Server.Authentication.DataTransferObjects;
using RandomApp.Server.Authentication.Models;

namespace RandomApp.Server.Authentication.Mapping
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>()
                            // if FirstName is null or empty return Unknown user else return FirstName
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FirstName) ? "Unknown User" : src.FirstName))
                            // if LastName is null or empty return Unknown user else return LastName
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.LastName) ? "Unknown User" : src.LastName))
                            // if UserName is null or empty return Unknown user else return UserName
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.UserName) ? "Unknown User" : src.UserName))
                            // if Email is null or empty return Unknown else return Email
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Email) ? "Unknown" : src.Email))
                            // if PhoneNumber is null or empty return Unknown else return PhoneNumber
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.PhoneNumber) ? "Unknown" : src.PhoneNumber));

        }


    }
}
