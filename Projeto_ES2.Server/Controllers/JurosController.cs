using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;

namespace Projeto_ES2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JurosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JurosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/juros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Juros>>> GetJuros()
        {
            return await _context.Juros
                .Include(j => j.FundoInvestimento)
                .ToListAsync();
        }

        // GET: api/juros/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Juros>> GetJuro(Guid id)
        {
            var juro = await _context.Juros
                .Include(j => j.FundoInvestimento)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (juro == null)
                return NotFound();

            return juro;
        }

        // POST: api/juros
        [HttpPost]
        public async Task<ActionResult<Juros>> PostJuro(Juros juros)
        {
            var fundo = await _context.FundosInvestimentos.FindAsync(juros.FundoId);
            if (fundo == null)
                return BadRequest("Fundo de investimento não encontrado.");

            _context.Juros.Add(juros);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJuro), new { id = juros.Id }, juros);
        }

        // PUT: api/juros/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJuro(Guid id, Juros juros)
        {
            if (id != juros.Id)
                return BadRequest();

            var fundo = await _context.FundosInvestimentos.FindAsync(juros.FundoId);
            if (fundo == null)
                return BadRequest("Fundo de investimento não encontrado.");

            _context.Entry(juros).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Juros.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/juros/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJuro(Guid id)
        {
            var juros = await _context.Juros.FindAsync(id);
            if (juros == null)
                return NotFound();

            _context.Juros.Remove(juros);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
