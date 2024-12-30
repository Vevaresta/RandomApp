using Microsoft.AspNetCore.Identity;
using RandomApp.Server.Authentication.DataTransferObjects;
using NLog;
using AutoMapper;
using RandomApp.Server.Authentication.Models;
using Microsoft.Extensions.Configuration;

namespace RandomApp.Server.Authentication.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;


        public AuthenticationService(ILogger logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);

            _logger.Debug("Attempting to create user {Username}", userForRegistration.UserName);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if (result.Succeeded && userForRegistration.Roles != null)
            {
                await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            }

            return result;
        }
    }
}
