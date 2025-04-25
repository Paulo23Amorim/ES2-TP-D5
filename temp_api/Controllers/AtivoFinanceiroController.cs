using Microsoft.AspNetCore.Authorization;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.DTOs;

namespace Projeto_ES2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AtivoFinanceiroController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AtivoFinanceiroController(ApplicationDbContext context)
    {
        _context = context;
    }

    // READ (Obter todos os ativos financeiros)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AtivoFinanceiro>>> GetAtivosFinanceiros()
    {
        return await _context.AtivosFinanceiros
            .Include(a => a.DepositoPrazo)
            .Include(a => a.FundoInvestimento)
            .Include(a => a.ImovelArrendado)
            .ToListAsync();
    }

    // READ (Obter um único ativo financeiro)
    [HttpGet("{id}")]
    public async Task<ActionResult<AtivoFinanceiro>> GetAtivoFinanceiro(Guid id)
    {
        var ativo = await _context.AtivosFinanceiros
            .Include(a => a.DepositoPrazo)
            .Include(a => a.FundoInvestimento)
            .Include(a => a.ImovelArrendado)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (ativo == null)
        {
            return NotFound();
        }

        return ativo;
    }

    // CREATE (Criar um novo ativo financeiro direto)
    [HttpPost]
    public async Task<ActionResult<AtivoFinanceiro>> CreateAtivoFinanceiro(AtivoFinanceiro ativo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.AtivosFinanceiros.Add(ativo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAtivoFinanceiro), new { id = ativo.Id }, ativo);
    }

        // CREATE COM DTO (p formulário do frontend)
    [HttpPost("novo")]
    [Authorize(Roles = "Admin,UserManager")] 
    public async Task<IActionResult> CriarComDto([FromBody] AtivoFinanceiroNovoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Tenta obter o ID do utilizador autenticado
        var userIdString = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;

        if (userIdString == null || !Guid.TryParse(userIdString, out var utilizadorId))
            return Unauthorized("Utilizador não autenticado ou ID inválido.");

        var ativo = new AtivoFinanceiro
        {
            Id = Guid.NewGuid(),
            UtilizadorId = utilizadorId,
            Nome = dto.Nome,
            Tipo = dto.Tipo,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim,
            Imposto = dto.Imposto
        };

        // Adiciona os dados específicos conforme o tipo
        switch (dto.Tipo)
        {
            case TipoAtivoFinanceiro.DepositoPrazo:
                if (dto.Deposito is not null)
                {
                    ativo.DepositoPrazo = new DepositoPrazo
                    {
                        Id = Guid.NewGuid(),
                        ValorInicial = dto.Deposito.ValorInicial,
                        Banco = dto.Deposito.Banco,
                        NumeroConta = dto.Deposito.NumeroConta,
                        Titulares = "Titular Padrão",
                        TaxaJuroAnual = dto.Deposito.TaxaJuroAnual,
                        AtivoFinanceiro = ativo
                    };
                }
                break;

            case TipoAtivoFinanceiro.FundoInvestimento:
                if (dto.Fundo is not null)
                {
                    ativo.FundoInvestimento = new FundoInvestimento
                    {
                        Id = Guid.NewGuid(),
                        MontanteInvestido = dto.Fundo.Montante,
                        TaxaJuroPadrao = dto.Fundo.TaxaJuroPadrao,
                        AtivoFinanceiro = ativo
                    };
                }
                break;

        case TipoAtivoFinanceiro.ImovelArrendado:
            if (dto.Imovel is not null)
            {
                ativo.ImovelArrendado = new ImovelArrendado
                {
                    Id = Guid.NewGuid(),
                    Localizacao = dto.Imovel.Localizacao,
                    ValorImovel = dto.Imovel.Valor,
                    ValorRenda = dto.Imovel.Renda,
                    ValorCondominio = dto.Imovel.Condominio,
                    DespesasAnuais = dto.Imovel.Despesas,
                    AtivoFinanceiro = ativo
                };
            }
            break;
    }

    _context.AtivosFinanceiros.Add(ativo);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetAtivoFinanceiro), new { id = ativo.Id }, ativo);
}

    // UPDATE (Atualizar um ativo financeiro)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAtivoFinanceiro(Guid id, AtivoFinanceiro ativo)
    {
        if (id != ativo.Id)
        {
            return BadRequest();
        }

        _context.Entry(ativo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AtivoFinanceiroExists(id))
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

    // DELETE (Remover um ativo financeiro)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Proteção por role

    public async Task<IActionResult> DeleteAtivoFinanceiro(Guid id)
    {
        var ativo = await _context.AtivosFinanceiros.FindAsync(id);
        if (ativo == null)
        {
            return NotFound();
        }

        _context.AtivosFinanceiros.Remove(ativo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Método auxiliar
    private bool AtivoFinanceiroExists(Guid id)
    {
        return _context.AtivosFinanceiros.Any(e => e.Id == id);
    }
}
