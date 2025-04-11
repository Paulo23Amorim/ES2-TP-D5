using Projeto_ES2.Components.Pages.DTOs;

namespace Projeto_ES2.Controllers;

// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Projeto_ES2.Components.Models;
using Projeto_ES2.Components.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var user = await _authService.LoginAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized();

        return Ok(user); // ou token/jwt/etc.
    }


    [HttpPost("register")]
    public IActionResult Register([FromBody] Utilizador novo)
    {
        var success = _authService.Register(novo);
        if (!success)
            return BadRequest("Email já está em uso.");

        return Ok("Registo efetuado com sucesso.");
    }
}
