using RandomApp.Server.Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;
using RandomApp.Server.Authentication.DataTransferObjects;

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
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _authenticationService.RegisterUser(userForRegistration);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }

    }
        
}
