using Microsoft.AspNetCore.Mvc;
using NLog;
using RandomApp.SharedKernel.Authentication.Application.DataTransferObjects;
using RandomApp.SharedKernel.Authentication.Application.Interfaces;

namespace RandomApp.Presentation.Api.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly NLog.ILogger _logger;
        private readonly IAuthenticationService _authenticationService;

        public TokenController(NLog.ILogger logger, IAuthenticationService authenticationService)
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
