using Microsoft.AspNetCore.Identity;
using RandomApp.SharedKernel.Authentication.Application.DataTransferObjects;

namespace RandomApp.SharedKernel.Authentication.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateToken(bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
