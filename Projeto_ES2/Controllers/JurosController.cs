using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JurosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public JurosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Juros
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Juros>>> GetJuros()
    {
        return await _context.Juros.ToListAsync();
    }

    // GET: api/Juros/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Juros>> GetJuros(int id)
    {
        var juros = await _context.Juros.FindAsync(id);

        if (juros == null)
        {
            return NotFound();
        }

        return juros;
    }
}