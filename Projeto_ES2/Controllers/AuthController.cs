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
    public IActionResult Login([FromBody] Utilizador user)
    {
        var result = _authService.Login(user.email, user.password);
        if (result == null)
            return Unauthorized("Email ou password inválidos.");

        return Ok(result);
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
