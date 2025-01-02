using Microsoft.AspNetCore.Identity;
using RandomApp.Server.Authentication.DataTransferObjects;
using NLog;
using AutoMapper;
using RandomApp.Server.Authentication.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace RandomApp.Server.Authentication.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User? _user;


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

            if (result.Succeeded && userForRegistration.Roles != null && userForRegistration.Roles.Any())
            {
                await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            }

            return result;
        }


        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName);

            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));

            if (!result)
            {
                _logger.Warn("{ValidateUser}: Authentication failed. Wrong user name or password.", nameof(ValidateUser));
            }

            return result;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
            {
                _logger.Error("JWT secret key is null or empty");
                throw new InvalidOperationException("JWT secret key is not configured");
            }

            var key = Encoding.UTF8.GetBytes(secretKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var issuer = jwtSettings["validIssuer"];
            var audience = jwtSettings["validAudience"];
            var expires = DateTime.Now.AddHours(Convert.ToDouble(jwtSettings["expires"]));

            _logger.Info("Generating token with Issuer: {0}, Audience: {1}, Expiry: {2}",
                issuer, audience, expires);

            var tokenOptions = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
    }
}
