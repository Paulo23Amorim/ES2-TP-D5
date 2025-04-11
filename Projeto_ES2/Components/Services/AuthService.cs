using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Components.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Utilizador?> LoginAsync(string email, string password)
    {
        return await _context.Utilizadores
            .FirstOrDefaultAsync(u => u.email == email && u.password == password);
    }


    public bool Register(Utilizador novo)
    {
        if (_context.Utilizadores.Any(u => u.email == novo.email))
            return false;

        _context.Utilizadores.Add(novo);
        _context.SaveChanges();
        return true;
    }

}