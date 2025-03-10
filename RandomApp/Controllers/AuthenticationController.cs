using Microsoft.AspNetCore.Mvc;
using NLog;
using Microsoft.AspNetCore.Http;
using RandomApp.Presentation.Authentication.DataTransferObjects;
using RandomApp.Presentation.Authentication.Services;

namespace RandomApp.Presentation.Api.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly NLog.ILogger _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(NLog.ILogger logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;

        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            _logger.Info("Starting user registration process for user {UserName}", userForRegistration.UserName);
            var result = await _authenticationService.RegisterUser(userForRegistration);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                    _logger.Warn("Registration failed: {ErrorCode} - {ErrorDescription}",
                        error.Code,
                        error.Description);
                }

                _logger.Error("Error registrating user {UserName}", userForRegistration.UserName);
                return BadRequest(ModelState);
            }

            _logger.Info("User successfuly registered: {UserName}", userForRegistration.UserName);
            return Created();
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authenticate(UserForAuthenticationDto user)
        {
            _logger.Info("Authenticating attempt for user: {Username}", user.UserName);
            if (!await _authenticationService.ValidateUser(user))
            {
                _logger.Warn("Authentication failed for user: {Username}", user.UserName);
                return Unauthorized();
            }

            var tokenDto = await _authenticationService.CreateToken(populateExp: true);
            _logger.Info("Token successfully created for user: {Username}", user.UserName);
            return Ok(tokenDto);
        }


    }

}
