using AutoMapper;
using RandomApp.SharedKernel.Authentication.Application.DataTransferObjects;
using RandomApp.SharedKernel.Authentication.Domain.Models;

namespace RandomApp.SharedKernel.Authentication.Application.Mapping
{
    public class AuthMappingProfile : Profile
    {
        private const string DefaultUnknownValue = "Unknown User";
        private const string DefaultUnknownContact = "Unknown";
        public AuthMappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>()
                // if FirstName is null or empty return Unknown user else return FirstName
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.FirstName) ? DefaultUnknownValue : src.FirstName))
                // if LastName is null or empty return Unknown user else return LastName
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.LastName) ? DefaultUnknownValue : src.LastName))
                // if UserName is null or empty return Unknown user else return UserName
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.UserName) ? DefaultUnknownValue : src.UserName))
                // if Email is null or empty return Unknown else return Email
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Email) ? DefaultUnknownContact : src.Email))
                // if PhoneNumber is null or empty return Unknown else return PhoneNumber
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.PhoneNumber) ? DefaultUnknownContact : src.PhoneNumber));

        }


    }
}
