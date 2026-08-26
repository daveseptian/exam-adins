using CutiApp.DTOs;
using CutiApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CutiApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            LoginResponse? result = await _authService.LoginAsync(request);
            if (result == null)
                return Unauthorized(new { status = "error", message = "Username atau Password salah." });
            return Ok(result);
        }

        [HttpGet("debug-claims")]
        public IActionResult DebugClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value, c.Issuer });
            var isInManagerRole = User.IsInRole("Manager");

            return Ok(new
            {
                IsInManagerRole = isInManagerRole,
                ClaimList = claims
            });
        }
    }
}
