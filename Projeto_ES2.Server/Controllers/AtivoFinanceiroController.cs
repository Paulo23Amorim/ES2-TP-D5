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
    [Authorize]
    public async Task<ActionResult<IEnumerable<AtivoFinanceiro>>> GetAtivosFinanceiros()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("ID do utilizador inválido ou ausente.");

        var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.user_id == userId);
        if (utilizador == null)
            return NotFound("Utilizador não encontrado.");

        var query = _context.AtivosFinanceiros
            .Include(a => a.DepositoPrazo)
            .Include(a => a.FundoInvestimento)
            .Include(a => a.ImovelArrendado)
            .Include(a => a.Utilizador) // <- Isto tem de vir ANTES de qualquer .Where()
            .AsQueryable(); // Garante tipo correto

        // Aplica filtro SE não for admin
        if (utilizador.TipoUtilizador != TipoUtilizador.Admin)
        {
            query = query.Where(a => a.UtilizadorId == userId);
        }

        return await query.ToListAsync();
    }




    [HttpGet("{id}")]
    public async Task<ActionResult<AtivoFinanceiro>> GetAtivoFinanceiro(Guid id)
    {
        var ativo = await _context.AtivosFinanceiros
            .Include(a => a.DepositoPrazo)
            .Include(a => a.FundoInvestimento)
            .Include(a => a.ImovelArrendado)
            .FirstOrDefaultAsync(a => a.Id == id);

        return ativo == null ? NotFound() : Ok(ativo);
    }

    [HttpPost("novo")]
    [Authorize]
    public async Task<IActionResult> CriarComDto([FromBody] AtivoFinanceiroNovoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!User.Identity.IsAuthenticated)
            return Unauthorized("Usuário não autenticado");

        Guid utilizadorId;

        if (dto.UtilizadorId.HasValue)
        {
            utilizadorId = dto.UtilizadorId.Value;
        }
        else
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) 
                              ?? User.FindFirst("sub") 
                              ?? User.FindFirst("id");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out utilizadorId))
                return Unauthorized("ID do usuário inválido ou não encontrado");
        }


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
                        ValorImovel = dto.Imovel.ValorImovel,
                        ValorRenda = dto.Imovel.ValorRenda,
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
[Authorize]
public async Task<IActionResult> UpdateAtivoFinanceiro(Guid id, [FromBody] AtivoFinanceiro ativo)
{
    if (id != ativo.Id)
        return BadRequest("ID inválido");

    var existente = await _context.AtivosFinanceiros
        .Include(a => a.DepositoPrazo)
        .Include(a => a.FundoInvestimento)
        .Include(a => a.ImovelArrendado)
        .FirstOrDefaultAsync(a => a.Id == id);

    if (existente == null)
        return NotFound();

    existente.Nome = ativo.Nome;
    existente.DataFim = ativo.DataFim;
    existente.Imposto = ativo.Imposto;

    switch (ativo.Tipo)
    {
        case TipoAtivoFinanceiro.FundoInvestimento:
            if (ativo.FundoInvestimento != null && existente.FundoInvestimento != null)
            {
                existente.FundoInvestimento.MontanteInvestido = ativo.FundoInvestimento.MontanteInvestido;
                existente.FundoInvestimento.TaxaJuroPadrao = ativo.FundoInvestimento.TaxaJuroPadrao;
            }
            break;

        case TipoAtivoFinanceiro.DepositoPrazo:
            if (ativo.DepositoPrazo != null && existente.DepositoPrazo != null)
            {
                existente.DepositoPrazo.ValorInicial = ativo.DepositoPrazo.ValorInicial;
                existente.DepositoPrazo.TaxaJuroAnual = ativo.DepositoPrazo.TaxaJuroAnual;
                existente.DepositoPrazo.Banco = ativo.DepositoPrazo.Banco;
            }
            break;

        case TipoAtivoFinanceiro.ImovelArrendado:
            if (ativo.ImovelArrendado != null && existente.ImovelArrendado != null)
            {
                existente.ImovelArrendado.Localizacao = ativo.ImovelArrendado.Localizacao;
                existente.ImovelArrendado.ValorImovel = ativo.ImovelArrendado.ValorImovel;
                existente.ImovelArrendado.ValorRenda = ativo.ImovelArrendado.ValorRenda;
            }
            break;
    }

    await _context.SaveChangesAsync();
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