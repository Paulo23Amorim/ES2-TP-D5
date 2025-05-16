using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpostoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImpostoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/imposto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Imposto>>> GetImpostos()
        {
            return await _context.Impostos
                .Include(i => i.AtivoFinanceiro)
                .ToListAsync();
        }

        // GET: api/imposto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Imposto>> GetImposto(Guid id)
        {
            var imposto = await _context.Impostos
                .Include(i => i.AtivoFinanceiro)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (imposto == null)
            {
                return NotFound();
            }

            return imposto;
        }

        // POST: api/imposto
        [HttpPost]
        public async Task<ActionResult<Imposto>> CreateImposto(Imposto imposto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Impostos.Add(imposto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImposto), new { id = imposto.Id }, imposto);
        }

        // PUT: api/imposto/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImposto(Guid id, Imposto imposto)
        {
            if (id != imposto.Id)
            {
                return BadRequest();
            }

            _context.Entry(imposto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImpostoExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/imposto/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImposto(Guid id)
        {
            var imposto = await _context.Impostos.FindAsync(id);
            if (imposto == null)
            {
                return NotFound();
            }

            _context.Impostos.Remove(imposto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImpostoExists(Guid id)
        {
            return _context.Impostos.Any(i => i.Id == id);
        }
    }
}