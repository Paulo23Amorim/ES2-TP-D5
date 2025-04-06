using Projeto_ES2.Components.Data; 
using Projeto_ES2.Components.Models; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;

namespace Projeto_ES2.Controllers;

[Route("api/[controller]")] 
[ApiController] 

public class AtivoFinanceiroController : ControllerBase {
    
private readonly ApplicationDbContext _context;

public AtivoFinanceiroController(ApplicationDbContext context)
    {
        _context = context;
    }

    // READ (Obter todos os ativos financeiros)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AtivoFinanceiro>>> GetAtivosFinanceiros()
    {
        return await _context.AtivosFinanceiros.ToListAsync();
    }

    // READ (Obter um único ativo financeiro)
    [HttpGet("{id}")]
    public async Task<ActionResult<AtivoFinanceiro>> GetAtivoFinanceiro(Guid id)
    {
        var ativo = await _context.AtivosFinanceiros.FindAsync(id);
        if (ativo == null)
        {
            return NotFound();
        }
        return ativo;
    }

    // CREATE (Criar um novo ativo financeiro)
    [HttpPost]
    public async Task<ActionResult<AtivoFinanceiro>> CreateAtivoFinanceiro(AtivoFinanceiro ativo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.AtivosFinanceiros.Add(ativo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAtivoFinanceiro), new { id = ativo.Id }, ativo);
    }

    // UPDATE (Atualizar um ativo financeiro)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAtivoFinanceiro(Guid id, AtivoFinanceiro ativo)
    {
        if (id != ativo.Id)
        {
            return BadRequest();
        }

        _context.Entry(ativo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AtivoFinanceiroExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE (Remover um ativo financeiro)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAtivoFinanceiro(Guid id)
    {
        var ativo = await _context.AtivosFinanceiros.FindAsync(id);
        if (ativo == null)
        {
            return NotFound();
        }

        _context.AtivosFinanceiros.Remove(ativo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Método auxiliar para verificar se um ativo financeiro existe
    private bool AtivoFinanceiroExists(Guid id)
    {
        return _context.AtivosFinanceiros.Any(e => e.Id == id);
    }
}
