using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;
using BCrypt.Net;

namespace Projeto_ES2.Components.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<Utilizador?> LoginAsync(string email, string password)
    {
        var user = await _context.Utilizadores
            .FirstOrDefaultAsync(u => u.email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }

    public string GenerateJwtToken(Utilizador user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing in configuration")));
        
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Name, user.nome),
            new Claim(ClaimTypes.Role, user.TipoUtilizador.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool Register(Utilizador novoUtilizador, string password)
    {
        if (_context.Utilizadores.Any(u => u.email == novoUtilizador.email))
            return false;

        novoUtilizador.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        _context.Utilizadores.Add(novoUtilizador);
        _context.SaveChanges();

        return true;
    }
}