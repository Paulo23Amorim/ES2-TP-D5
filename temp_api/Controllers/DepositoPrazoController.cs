using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;
using Projeto_ES2.Components.Pages.DTOs;

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
    public async Task<ActionResult<IEnumerable<DepositoPrazoDto>>> GetAll()
    {
        var depositos = await _context.DepositosPrazos
            .Include(d => d.AtivoFinanceiro)
            .ToListAsync();

        return depositos.Select(ToDto).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepositoPrazoDto>> GetById(Guid id)
    {
        var deposito = await _context.DepositosPrazos
            .Include(d => d.AtivoFinanceiro)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (deposito == null) return NotFound();

        return ToDto(deposito);
    }

    [HttpPost]
    public async Task<IActionResult> PostDepositoPrazo(DepositoPrazoCreateDto dto)
    {
        var ativo = new AtivoFinanceiro {
            Nome = dto.AtivoFinanceiro.Nome,
            DataInicio = dto.AtivoFinanceiro.DataInicio,
            DataFim = dto.AtivoFinanceiro.DataFim,
            Tipo = (TipoAtivoFinanceiro)dto.AtivoFinanceiro.Tipo,
            Imposto = dto.AtivoFinanceiro.Imposto,
            UtilizadorId = dto.AtivoFinanceiro.UtilizadorId
        };

        var deposito = new DepositoPrazo {
            Banco = dto.Banco,
            NumeroConta = dto.NumeroConta,
            TaxaJuroAnual = dto.TaxaJuroAnual,
            ValorInicial = dto.ValorInicial,
            Titulares = dto.Titulares,
            AtivoFinanceiro = ativo
        };

        _context.DepositosPrazos.Add(deposito);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = deposito.Id }, ToDto(deposito));
    }




    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, DepositoPrazoDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var deposito = FromDto(dto);
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
    
    private static DepositoPrazoDto ToDto(DepositoPrazo deposito)
    {
        return new DepositoPrazoDto
        {
            Id = deposito.Id,
            ValorInicial = deposito.ValorInicial,
            NumeroConta = deposito.NumeroConta,
            Titulares = deposito.Titulares,
            TaxaJuroAnual = deposito.TaxaJuroAnual
        };
    }

    private static DepositoPrazo FromDto(DepositoPrazoDto dto)
    {
        return new DepositoPrazo
        {
            Id = dto.Id,
            ValorInicial = dto.ValorInicial,
            NumeroConta = dto.NumeroConta,
            Titulares = dto.Titulares,
            TaxaJuroAnual = dto.TaxaJuroAnual,
            Banco = dto.Banco,
            AtivoFinanceiro = dto.AtivoFinanceiro
        };
    }

}
