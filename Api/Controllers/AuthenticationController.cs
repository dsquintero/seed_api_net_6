using Api.DTOs;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(
            IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            return Ok(await authenticationService.Login(loginRequest));
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            return Ok(await authenticationService.Refresh(refreshRequest));
        }

        [Authorize]
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            string UserCode = HttpContext.User.FindFirstValue("UserCode");
            string CountryCode = HttpContext.User.FindFirstValue(ClaimTypes.Country);

            await authenticationService.Logout(UserCode);
            return NoContent();
        }
    }
}
