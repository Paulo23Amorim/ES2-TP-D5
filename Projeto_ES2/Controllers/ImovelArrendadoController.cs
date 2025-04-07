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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ImovelArrendado>>> GetAll()
    {
        return await _context.ImovelArrendados
            .Include(i => i.AtivoFinanceiro)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ImovelArrendado>> GetById(Guid id)
    {
        var imovel = await _context.ImovelArrendados
            .Include(i => i.AtivoFinanceiro)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (imovel == null) return NotFound();

        return imovel;
    }

    [HttpPost]
    public async Task<ActionResult<ImovelArrendado>> Create(ImovelArrendado imovel)
    {
        _context.ImovelArrendados.Add(imovel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = imovel.Id }, imovel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ImovelArrendado imovel)
    {
        if (id != imovel.Id) return BadRequest();

        _context.Entry(imovel).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.ImovelArrendados.Any(i => i.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var imovel = await _context.ImovelArrendados.FindAsync(id);
        if (imovel == null) return NotFound();

        _context.ImovelArrendados.Remove(imovel);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
