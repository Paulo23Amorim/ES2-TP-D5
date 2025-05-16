using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.DTOs;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;

namespace Projeto_ES2.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AtivoFinanceiroController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AtivoFinanceiroController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AtivoFinanceiro>>> GetAtivosFinanceiros()
    {
        return await _context.AtivosFinanceiros
            .Include(a => a.DepositoPrazo)
            .Include(a => a.FundoInvestimento)
            .Include(a => a.ImovelArrendado)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AtivoFinanceiro>> GetAtivoFinanceiro(Guid id)
    {
        var ativo = await _context.AtivosFinanceiros
            .Include(a => a.DepositoPrazo)
            .Include(a => a.FundoInvestimento)
            .Include(a => a.ImovelArrendado)
            .FirstOrDefaultAsync(a => a.Id == id);

        return ativo == null ? NotFound() : ativo;
    }

    [HttpPost("novo")]
    [Authorize(Roles = "Admin,UserManager")]
    public async Task<IActionResult> CriarComDto([FromBody] AtivoFinanceiroNovoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!User.Identity.IsAuthenticated)
            return Unauthorized("Usuário não autenticado");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) 
                        ?? User.FindFirst("sub") 
                        ?? User.FindFirst("id");
    
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var utilizadorId))
            return Unauthorized("ID do usuário inválido ou não encontrado");

        var ativo = new AtivoFinanceiro
        {
            Id = Guid.NewGuid(),
            UtilizadorId = utilizadorId,
            Nome = dto.Nome,
            Tipo = dto.Tipo,
            DataInicio = dto.DataInicio.ToUniversalTime(),
            DataFim = dto.DataFim?.ToUniversalTime(), // Usando null-conditional operator
            Imposto = dto.Imposto
        };

        // Adiciona os dados específicos conforme o tipo
        switch (dto.Tipo)
        {
            case TipoAtivoFinanceiro.DepositoPrazo:
                if (dto.Deposito != null)
                {
                    ativo.DepositoPrazo = new DepositoPrazo
                    {
                        Id = Guid.NewGuid(),
                        ValorInicial = dto.Deposito.ValorInicial,
                        Banco = dto.Deposito.Banco,
                        NumeroConta = dto.Deposito.NumeroConta,
                        Titulares = dto.Deposito.Titulares,
                        TaxaJuroAnual = dto.Deposito.TaxaJuroAnual,
                        AtivoId = ativo.Id,
                        AtivoFinanceiro = null
                    };
                }
                break;

            case TipoAtivoFinanceiro.FundoInvestimento:
                if (dto.Fundo != null)
                {
                    ativo.FundoInvestimento = new FundoInvestimento
                    {
                        Id = Guid.NewGuid(),
                        MontanteInvestido = dto.Fundo.MontanteInvestido,
                        TaxaJuroPadrao = dto.Fundo.TaxaJuroPadrao,
                        AtivoId = ativo.Id,
                        AtivoFinanceiro = null
                    };
                }
                break;

            case TipoAtivoFinanceiro.ImovelArrendado:
                if (dto.Imovel != null)
                {
                    ativo.ImovelArrendado = new ImovelArrendado
                    {
                        Id = Guid.NewGuid(),
                        Localizacao = dto.Imovel.Localizacao,
                        ValorImovel = dto.Imovel.Valor,
                        ValorRenda = dto.Imovel.Renda,
                        ValorCondominio = dto.Imovel.Condominio,
                        DespesasAnuais = dto.Imovel.Despesas,
                        AtivoId = ativo.Id,
                        AtivoFinanceiro = null
                    };
                }
                break;
        }

        _context.AtivosFinanceiros.Add(ativo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAtivoFinanceiro), new { id = ativo.Id }, ativo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAtivoFinanceiro(Guid id, AtivoFinanceiro ativo)
    {
        if (id != ativo.Id || !ModelState.IsValid)
            return BadRequest();

        _context.Entry(ativo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AtivoFinanceiroExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAtivoFinanceiro(Guid id)
    {
        var ativo = await _context.AtivosFinanceiros.FindAsync(id);
        if (ativo == null)
            return NotFound();

        _context.AtivosFinanceiros.Remove(ativo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AtivoFinanceiroExists(Guid id)
    {
        return _context.AtivosFinanceiros.Any(e => e.Id == id);
    }
}