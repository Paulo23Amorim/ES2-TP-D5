using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FundoInvestimentoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FundoInvestimentoController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/FundoInvestimento
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FundoInvestimento>>> GetFundos()
    {
        return await _context.FundosInvestimentos
            .Include(f => f.AtivoFinanceiro)
            .Include(f => f.Juros)
            .ToListAsync();
    }

    // GET: api/FundoInvestimento/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<FundoInvestimento>> GetFundo(Guid id)
    {
        var fundo = await _context.FundosInvestimentos
            .Include(f => f.AtivoFinanceiro)
            .Include(f => f.Juros)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fundo == null)
            return NotFound();

        return fundo;
    }

    // POST: api/FundoInvestimento
    [HttpPost]
    public async Task<ActionResult<FundoInvestimento>> PostFundo(FundoInvestimento fundo)
    {
        _context.FundosInvestimentos.Add(fundo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFundo), new { id = fundo.Id }, fundo);
    }

    // PUT: api/FundoInvestimento/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFundo(Guid id, FundoInvestimento fundo)
    {
        if (id != fundo.Id)
            return BadRequest();

        _context.Entry(fundo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.FundosInvestimentos.Any(f => f.Id == id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    // DELETE: api/FundoInvestimento/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFundo(Guid id)
    {
        var fundo = await _context.FundosInvestimentos.FindAsync(id);
        if (fundo == null)
            return NotFound();

        _context.FundosInvestimentos.Remove(fundo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
