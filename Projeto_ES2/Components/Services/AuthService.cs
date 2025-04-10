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

    public Utilizador? Login(string email, string password)
    {
        return _context.Utilizadores
            .FirstOrDefault(u => u.email == email && u.password == password);
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