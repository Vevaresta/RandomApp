﻿using Microsoft.AspNetCore.Identity;
using RandomApp.Presentation.Authentication.DataTransferObjects;

namespace RandomApp.Presentation.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateToken(bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
