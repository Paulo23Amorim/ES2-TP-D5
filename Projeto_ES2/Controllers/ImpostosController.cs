using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImpostosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ImpostosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Impostos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Impostos>>> GetImpostos()
    {
        return await _context.Impostos.ToListAsync();
    }

    // GET: api/Impostos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Impostos>> GetImposto(Guid id)
    {
        var imposto = await _context.Impostos.FindAsync(id);

        if (imposto == null)
        {
            return NotFound();
        }

        return imposto;
    }
}