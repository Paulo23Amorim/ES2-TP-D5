using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.DTOs;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;

namespace Projeto_ES2.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FundoInvestimentoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FundoInvestimentoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FundoInvestimento>>> GetFundos()
    {
        return await _context.FundosInvestimentos
            .Include(f => f.AtivoFinanceiro)
            .Include(f => f.Juros)
            .ToListAsync();
    }

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

    [HttpPost]
    public async Task<ActionResult<FundoInvestimento>> PostFundo([FromBody] FundoInvestimentoDTO fundoDto)
    {
        // Validação do modelo
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Criação do AtivoFinanceiro
        var ativoFinanceiro = new AtivoFinanceiro
        {
            Id = Guid.NewGuid(),
            Nome = "Fundo de Investimento",
            Tipo = TipoAtivoFinanceiro.FundoInvestimento,
            DataInicio = DateTime.Now,
            Imposto = 0.10m // Valor padrão para impostos em fundos
        };

        // Criação do Fundo
        var fundo = new FundoInvestimento
        {
            Id = Guid.NewGuid(),
            AtivoFinanceiro = ativoFinanceiro,
            MontanteInvestido = fundoDto.MontanteInvestido,
            TaxaJuroPadrao = fundoDto.TaxaJuroPadrao
        };

        _context.FundosInvestimentos.Add(fundo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFundo), new { id = fundo.Id }, fundo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutFundo(Guid id, [FromBody] FundoInvestimentoDTO fundoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var fundo = await _context.FundosInvestimentos
            .Include(f => f.AtivoFinanceiro)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fundo == null) return NotFound();

        // Mapeamento das propriedades
        fundo.MontanteInvestido = fundoDto.MontanteInvestido;
        fundo.TaxaJuroPadrao = fundoDto.TaxaJuroPadrao;

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var fundo = await _context.FundosInvestimentos.FindAsync(id);
        if (fundo == null)
            return NotFound();

        _context.FundosInvestimentos.Remove(fundo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}