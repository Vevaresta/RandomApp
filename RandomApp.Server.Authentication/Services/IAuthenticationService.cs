using Microsoft.AspNetCore.Identity;
using RandomApp.Server.Authentication.DataTransferObjects;

namespace RandomApp.Server.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<string> CreateToken();
    }
}
