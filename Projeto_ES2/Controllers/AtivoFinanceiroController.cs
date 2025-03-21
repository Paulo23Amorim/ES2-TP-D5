using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

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

    // GET: api/AtivoFinanceiro
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AtivoFinanceiro>>> GetAtivosFinanceiros()
    {
        return await _context.AtivosFinanceiros.ToListAsync();  // Nome da tabela que representa os AtivosFinanceiros
    }

    // GET: api/AtivoFinanceiro/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<AtivoFinanceiro>> GetAtivoFinanceiro(Guid id)
    {
        var ativoFinanceiro = await _context.AtivosFinanceiros.FindAsync(id);  // Busca pelo ID (Guid)

        if (ativoFinanceiro == null)
        {
            return NotFound();
        }

        return ativoFinanceiro;
    }
}