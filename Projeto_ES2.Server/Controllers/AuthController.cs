using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Projeto_ES2.Client.Components.DTOs;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;
using Projeto_ES2.Server.Services;

namespace Projeto_ES2.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly ApplicationDbContext _context;
    public AuthController(
        AuthService authService, 
        IConfiguration configuration,
        ILogger<AuthController> logger,
        ApplicationDbContext context)
    {
        _authService = authService;
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

    [HttpPost("login")]
[AllowAnonymous]
public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
{
    try
    {
        // 1. Validação mais rigorosa da chave JWT
        var jwtKey = _configuration["Jwt:Key"]?.Trim();
        if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
        {
            _logger.LogError("Chave JWT inválida. Tamanho: {Length}", jwtKey?.Length ?? 0);
            throw new InvalidOperationException("Configuração JWT inválida");
        }

        // 2. Autenticação do usuário
        var utilizador = await _authService.LoginAsync(request.Email, request.Password);
        if (utilizador == null)
        {
            _logger.LogWarning("Login falhou para {Email}", request.Email);
            return Unauthorized(new { Message = "Credenciais inválidas" });
        }

        // 3. Geração do token com tratamento de erros
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, utilizador.user_id.ToString()),
                    new Claim(ClaimTypes.Email, utilizador.email),
                    new Claim(ClaimTypes.Name, utilizador.nome),
                    new Claim(ClaimTypes.Role, utilizador.TipoUtilizador.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return Ok(new 
            {
                token = tokenHandler.WriteToken(token),
                user = new 
                {
                    utilizador.user_id,
                    utilizador.nome,
                    utilizador.email,
                    role = utilizador.TipoUtilizador.ToString()
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar token JWT");
            throw new InvalidOperationException("Falha ao gerar token de autenticação");
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro no processo de login");
        return StatusCode(500, new { 
            Message = "Erro interno no servidor",
            Detail = ex.Message // Adicionando detalhes do erro para desenvolvimento
        });
    }
}
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDTO request)
    {
        try
        {
            var novoUtilizador = new Utilizador
            {
                user_id = Guid.NewGuid(),
                nome = request.Name,
                email = request.Email,
                TipoUtilizador = request.Tipo,
                PasswordHash = string.Empty 
            };

            var resultado = await _authService.RegisterAsync(novoUtilizador, request.Password);
            
            if (!resultado)
                return Conflict(new { Message = "Email já está em uso" });

            return Ok(new { Message = "Registo efetuado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o registo");
            return StatusCode(500, new { Message = "Ocorreu um erro durante o registo" });
        }
    }

    [HttpGet("perfil")]
    [Authorize]
    public async Task<IActionResult> Perfil()
    {
        try
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            if (emailClaim == null || string.IsNullOrEmpty(emailClaim.Value))
                return Unauthorized();

            var utilizador = await _authService.BuscarPorEmailAsync(emailClaim.Value);
            
            if (utilizador == null)
                return NotFound();

            return Ok(new UserDTO
            {
                user_id = utilizador.user_id,
                nome = utilizador.nome,
                email = utilizador.email,
                Tipo = (int)utilizador.TipoUtilizador
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter perfil");
            return StatusCode(500, new { Message = "Ocorreu um erro ao obter o perfil" });
        }
    }
    
    public class ResetPasswordRequest
    {
        public Guid UserId { get; set; }
        public string NovaPassword { get; set; } = string.Empty;
    }


    private string GenerateJwtToken(Utilizador utilizador, string jwtKey)
    {
        var key = Encoding.UTF8.GetBytes(jwtKey);
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, utilizador.user_id.ToString()),
            new Claim(ClaimTypes.Email, utilizador.email),
            new Claim(ClaimTypes.Name, utilizador.nome),
            new Claim(ClaimTypes.Role, utilizador.TipoUtilizador.ToString())
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"] ?? "120")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}