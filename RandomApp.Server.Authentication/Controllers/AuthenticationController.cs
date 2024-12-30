using RandomApp.Server.Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;
using RandomApp.Server.Authentication.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace RandomApp.Server.Authentication.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ILogger logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
               
        }


        [HttpPost]
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

            _logger.Info("User sucsessfuly registered: {UserName}", userForRegistration.UserName);
            return Created();
        }

    }
        
}
