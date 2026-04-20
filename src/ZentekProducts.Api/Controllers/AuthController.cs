using Microsoft.AspNetCore.Mvc;
using ZentekProducts.Api.Models.DTOs;
using ZentekProducts.Api.Services;

namespace ZentekProducts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(AuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        var validUsername = _configuration["Auth:Username"] ?? "admin";
        var validPassword = _configuration["Auth:Password"] ?? "admin123";

        if (loginDto.Username != validUsername || loginDto.Password != validPassword)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = _authService.GenerateToken(loginDto.Username);
        return Ok(token);
    }
}