using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;
using System.Security.Claims;

namespace Projeto_ES2.Server.Controllers;

[Route("api/dashboard")]
[ApiController]
[Authorize(Roles = "Utilizador")]
public class DashboardClienteController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardClienteController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("impostos")]
    public async Task<IActionResult> GetImpostosSimulados(DateTime dataInicio, DateTime dataFim)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var ativosDoUser = await _context.AtivosFinanceiros
            .Include(a => a.DepositoPrazo)
            .Include(a => a.FundoInvestimento)
            .Include(a => a.ImovelArrendado)
            .Where(a => a.UtilizadorId.ToString() == userId)
            .ToListAsync();

        var resultados = new List<object>();

        foreach (var ativo in ativosDoUser)
        {
            var inicio = ativo.DataInicio < dataInicio ? dataInicio : ativo.DataInicio;
            var fim = ativo.DataFim > dataFim ? dataFim : ativo.DataFim;

            for (var date = new DateTime(inicio.Year, inicio.Month, 1); date <= fim; date = date.AddMonths(1))
            {
                decimal imposto = 0;

                switch (ativo.Tipo)
                {
                    case TipoAtivoFinanceiro.DepositoPrazo:
                        var d = ativo.DepositoPrazo;
                        if (d != null && d.TaxaJuroAnual > 0)
                            imposto = (d.ValorInicial * d.TaxaJuroAnual / 100) / 12;
                        break;

                    case TipoAtivoFinanceiro.FundoInvestimento:
                        var f = ativo.FundoInvestimento;
                        if (f != null && f.TaxaJuroPadrao > 0)
                            imposto = (f.MontanteInvestido * f.TaxaJuroPadrao / 100) / 12;
                        break;

                    case TipoAtivoFinanceiro.ImovelArrendado:
                        var i = ativo.ImovelArrendado;
                        if (i != null)
                            imposto = i.ValorRenda ?? 0;
                        break;
                }

                resultados.Add(new
                {
                    Ativo = ativo.Nome,
                    Mes = date.ToString("yyyy-MM"),
                    Total = Math.Round(imposto, 2)
                });
            }
        }

        return Ok(resultados.OrderBy(r => ((dynamic)r).Mes).ThenBy(r => ((dynamic)r).Ativo).ToList());
    }
}