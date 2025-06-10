using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;
using BCrypt.Net;

namespace Projeto_ES2.Server.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Utilizador?> LoginAsync(string email, string password)
    {
        var utilizador = await _context.Utilizadores
            .FirstOrDefaultAsync(u => u.email == email);
    
        if (utilizador == null || !BCrypt.Net.BCrypt.Verify(password, utilizador.PasswordHash))
            return null;
    
        return utilizador;
    }

    public async Task<bool> RegisterAsync(Utilizador user, string password)
    {
        if (await _context.Utilizadores.AnyAsync(u => u.email == user.email))
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        _context.Utilizadores.Add(user);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task<Utilizador?> BuscarPorEmailAsync(string email)
    {
        return await _context.Utilizadores
            .FirstOrDefaultAsync(u => u.email == email);
    }
}