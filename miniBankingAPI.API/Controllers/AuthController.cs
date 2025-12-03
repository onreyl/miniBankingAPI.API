using Microsoft.AspNetCore.Mvc;
using miniBankingAPI.Application.DTOs;
using miniBankingAPI.Domain.Interfaces;

namespace miniBankingAPI.API.Controllers
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
            var token = await _authService.Login(request.Username, request.Password);
            return Ok(new LoginResponse(token));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var userId = await _authService.Register(request.Username, request.Password, request.Email, request.CustomerId);
            return Ok(new RegisterResponse(userId));
        }
    }
}
