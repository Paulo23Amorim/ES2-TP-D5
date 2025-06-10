using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;

namespace Projeto_ES2.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilizadoresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UtilizadoresController(ApplicationDbContext context)
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
    [Authorize(Roles = "Admin,UserManager")]
    public async Task<ActionResult<Utilizador>> CreateUtilizador(Utilizador utilizador)
    {
        if (User.IsInRole("UserManager") && utilizador.TipoUtilizador != TipoUtilizador.Utilizador)
        {
            return Forbid("UserManager só pode criar utilizadores do tipo 'Utilizador' (cliente).");
        }

        utilizador.user_id = Guid.NewGuid();

        _context.Utilizadores.Add(utilizador);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUtilizador), new { id = utilizador.user_id }, utilizador);
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

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,UserManager")]
    public async Task<IActionResult> UpdateUtilizador(Guid id, Utilizador utilizador)
    {
        if (id != utilizador.user_id)
        {
            return BadRequest("ID do utilizador não corresponde.");
        }

        // Verifica se é UserManager e está a tentar editar algo que não pode
        if (User.IsInRole("UserManager") && utilizador.TipoUtilizador != TipoUtilizador.Utilizador)
        {
            return Forbid("UserManager só pode editar utilizadores do tipo 'Utilizador'.");
        }

        // Protege contra alteração forçada da role (ex: UserManager a mudar alguém para Admin)
        var existente = await _context.Utilizadores.AsNoTracking().FirstOrDefaultAsync(u => u.user_id == id);
        if (existente == null)
        {
            return NotFound("Utilizador não encontrado.");
        }

        if (User.IsInRole("UserManager") && existente.TipoUtilizador != TipoUtilizador.Utilizador)
        {
            return Forbid("UserManager não tem permissão para editar este utilizador.");
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