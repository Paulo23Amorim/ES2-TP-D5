using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Projeto_ES2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilizadorController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UtilizadorController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Utilizador>>> GetArtists()
    {
        return await _context.Utilizadores.ToListAsync();
    }
    
}