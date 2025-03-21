using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImovelArrendadoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ImovelArrendadoController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/ImovelArrendado
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ImovelArrendado>>> GetImoveisArrendados()
    {
        return await _context.ImoveisArrendados.ToListAsync();
    }

    // GET: api/ImovelArrendado/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ImovelArrendado>> GetImovelArrendado(int id)
    {
        var imovelArrendado = await _context.ImoveisArrendados.FindAsync(id);

        if (imovelArrendado == null)
        {
            return NotFound();
        }

        return imovelArrendado;
    }
}