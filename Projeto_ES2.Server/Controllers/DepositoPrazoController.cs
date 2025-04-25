using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.DTOs;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;

namespace Projeto_ES2.Server.Controllers;

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
    public async Task<ActionResult<DepositoPrazo>> Create([FromBody] DepositoPrazoDTO depositoDto)
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
            Nome = "Depósito - " + depositoDto.Banco,
            Tipo = TipoAtivoFinanceiro.DepositoPrazo,
            DataInicio = DateTime.Now,
            Imposto = 0.28m // Valor padrão para impostos em depósitos
        };

        // Criação do Depósito
        var deposito = new DepositoPrazo
        {
            Id = Guid.NewGuid(),
            AtivoFinanceiro = ativoFinanceiro,
            ValorInicial = depositoDto.ValorInicial,
            Banco = depositoDto.Banco,
            NumeroConta = depositoDto.NumeroConta,
            Titulares = depositoDto.Titulares,
            TaxaJuroAnual = depositoDto.TaxaJuroAnual
        };

        _context.DepositosPrazos.Add(deposito);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = deposito.Id }, deposito);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DepositoPrazoDTO depositoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var deposito = await _context.DepositosPrazos
            .Include(d => d.AtivoFinanceiro)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (deposito == null) return NotFound();

        // Mapeamento das propriedades
        deposito.ValorInicial = depositoDto.ValorInicial;
        deposito.Banco = depositoDto.Banco;
        deposito.NumeroConta = depositoDto.NumeroConta;
        deposito.Titulares = depositoDto.Titulares;
        deposito.TaxaJuroAnual = depositoDto.TaxaJuroAnual;

        // Atualização do nome no AtivoFinanceiro
        if (deposito.AtivoFinanceiro != null)
        {
            deposito.AtivoFinanceiro.Nome = "Depósito - " + depositoDto.Banco;
        }

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