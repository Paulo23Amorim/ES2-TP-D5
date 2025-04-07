using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Projeto_ES2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtilizadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Utilizador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizadores()
        {
            return await _context.Utilizadores.ToListAsync();
        }

        // GET: api/Utilizador/{id}
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

        // POST: api/Utilizador
        [HttpPost]
        public async Task<ActionResult<Utilizador>> PostUtilizador(Utilizador utilizador)
        {
            // Validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Adiciona o utilizador ao contexto
            _context.Utilizadores.Add(utilizador);

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();

            // Retorna a resposta com o utilizador recém-criado
            return CreatedAtAction(nameof(GetUtilizador), new { id = utilizador.Id }, utilizador);
        }

        // PUT: api/Utilizador/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizador(Guid id, Utilizador utilizador)
        {
            if (id != utilizador.Id)
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
                if (!GetUtilizadorExists(id))
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

        // DELETE: api/Utilizador/{id}
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

        private bool GetUtilizadorExists(Guid id)
        {
            return _context.Utilizadores.Any(e => e.Id == id);
        }
    }
}
