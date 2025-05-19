using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;
using Projeto_ES2.Server.Services;

namespace Projeto_ES2.Server.Controllers
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
        public IActionResult CalcularImposto(Guid id)
        {
            var ativo = _context.AtivosFinanceiros
                .Include(a => a.DepositoPrazo)
                .Include(a => a.FundoInvestimento)
                .Include(a => a.ImovelArrendado)
                .FirstOrDefault(a => a.Id == id);

            if (ativo == null) return NotFound();

            var calculadora = new CalculadoraImposto();
            ImpostoStrategyAtivos estrategia = null;

            switch (ativo.Tipo)
            {
                case TipoAtivoFinanceiro.DepositoPrazo:
                    estrategia = new ImpostoDepositoPrazo();
                    break;
                case TipoAtivoFinanceiro.FundoInvestimento:
                    estrategia = new ImpostoFundoInvestimento();
                    break;
                case TipoAtivoFinanceiro.ImovelArrendado:
                    estrategia = new ImpostoImovelArrendado();
                    break;
                default:
                    return BadRequest("Tipo de ativo financeiro não suportado");
            }

            calculadora.DefinirEstrategia(estrategia);
            var valor = calculadora.Calcular(ativo);
    
            return Ok(new { impostoCalculado = valor });
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

            return CreatedAtAction(nameof(GetImpostos), new { id = imposto.Id }, imposto);
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
