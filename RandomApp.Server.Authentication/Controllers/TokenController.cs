using Microsoft.AspNetCore.Mvc;
using NLog;
using RandomApp.Server.Authentication.DataTransferObjects;
using RandomApp.Server.Authentication.Services;

namespace RandomApp.Server.Authentication.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAuthenticationService _authenticationService;

        public TokenController(ILogger logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _authenticationService.RefreshToken(tokenDto);

            return Ok(tokenDtoToReturn);
        }


    }
}
