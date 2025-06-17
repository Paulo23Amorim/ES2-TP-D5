using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Server.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;


namespace Projeto_ES2.Server.Controllers;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("dashboard-estatisticas")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var totalUtilizadores = await _context.Utilizadores.CountAsync();

        var totalClientes = await _context.Utilizadores
            .Where(u => u.TipoUtilizador == TipoUtilizador.Utilizador)
            .CountAsync();
        
        var totalAtivos = await _context.AtivosFinanceiros.CountAsync();

        var ativosPorTipo = await _context.AtivosFinanceiros
            .GroupBy(a => a.Tipo)
            .Select(g => new { Tipo = g.Key, Quantidade = g.Count() })
            .ToListAsync();

        return Ok(new
        {
            TotalUtilizadores = totalUtilizadores,

            TotalClientes = totalClientes,
            TotalAtivos = totalAtivos,
            AtivosPorTipo = ativosPorTipo
        });
    }
}