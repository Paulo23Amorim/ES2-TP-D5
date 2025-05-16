using Microsoft.AspNetCore.Authorization;
using Projeto_ES2.Components.DTOs;
using Projeto_ES2.Components.Pages.DTOs;

namespace Projeto_ES2.Controllers;

using Microsoft.AspNetCore.Mvc;
using Projeto_ES2.Components.Models;
using Projeto_ES2.Components.Services;

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
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var user = await _authService.LoginAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized(new { Message = "Credenciais inválidas" });

        var token = _authService.GenerateJwtToken(user);
        
        return Ok(new {
            Token = token,
            User = new {
                user.user_id,
                user.nome,
                user.email,
                Tipo = user.TipoUtilizador.ToString() // Retorna o nome do enum
            }
        });
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin,UserManager")] // Apenas admins e gestores podem registrar
    public IActionResult Register([FromBody] RegisterDTO request)
    {
        // Validação adicional para tipos de usuário
        if (request.Tipo == TipoUtilizador.Admin && 
            !User.IsInRole(TipoUtilizador.Admin.ToString()))
        {
            return Forbid("Apenas administradores podem criar outros administradores");
        }

        var novoUtilizador = new Utilizador
        {
            nome = request.Name,
            email = request.Email,
           // PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            TipoUtilizador = request.Tipo
        };

        var success = _authService.Register(novoUtilizador,request.Password);
        if (!success)
            return Conflict(new { Message = "Email já está em uso" });

        return Ok(new { Message = "Registo efetuado com sucesso" });
    }
}