using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepositoPrazoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DepositoPrazoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepositoPrazo>>> GetAll()
    {
        return await _context.DepositosPrazos
            .Include(d => d.AtivoFinanceiro)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepositoPrazo>> GetById(Guid id)
    {
        var deposito = await _context.DepositosPrazos
            .Include(d => d.AtivoFinanceiro)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (deposito == null) return NotFound();

        return deposito;
    }

    [HttpPost]
    public async Task<ActionResult<DepositoPrazo>> Create(DepositoPrazo deposito)
    {
        _context.DepositosPrazos.Add(deposito);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = deposito.Id }, deposito);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, DepositoPrazo deposito)
    {
        if (id != deposito.Id) return BadRequest();

        _context.Entry(deposito).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.DepositosPrazos.Any(d => d.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deposito = await _context.DepositosPrazos.FindAsync(id);
        if (deposito == null) return NotFound();

        _context.DepositosPrazos.Remove(deposito);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
