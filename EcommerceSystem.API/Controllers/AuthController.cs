using EcommerceSystem.Application.Interfaces;
using EcommerceSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSystem.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(IUserService userService, JwtTokenGenerator tokenGenerator)
        {
            _userService = userService;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            var user = _userService.ValidateUser(dto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _tokenGenerator.GenerateToken(user);
            return Ok(new { token });
        }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
    }
}
