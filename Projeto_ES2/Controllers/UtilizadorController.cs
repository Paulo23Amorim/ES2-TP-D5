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
    public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizadores()
    {
        try
        {
            var utilizadores = await _context.Utilizadores.ToListAsync();

            if (utilizadores == null || !utilizadores.Any())
            {
                return NotFound("Nenhum utilizador encontrado.");
            }

            return Ok(utilizadores);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<Utilizador>> CreateUtilizador(Utilizador utilizador)
    {
        _context.Utilizadores.Add(utilizador);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUtilizadores), new { id = utilizador.user_id }, utilizador);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Utilizador>> GetUtilizador(Guid id)
    {
        var utilizador = await _context.Utilizadores.FindAsync(id);

        if (utilizador == null)
        {
            return NotFound();
        }

        return utilizador;
    }

    // UPDATE: Atualiza um utilizador existente
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUtilizador(Guid id, Utilizador utilizador)
    {
        if (id != utilizador.user_id)
        {
            return BadRequest();
        }

        _context.Entry(utilizador).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UtilizadorExists(id))
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

    // DELETE: Deleta um utilizador
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUtilizador(Guid id)
    {
        var utilizador = await _context.Utilizadores.FindAsync(id);
        if (utilizador == null)
        {
            return NotFound();
        }

        _context.Utilizadores.Remove(utilizador);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UtilizadorExists(Guid id)
    {
        return _context.Utilizadores.Any(e => e.user_id == id);
    }
    
}