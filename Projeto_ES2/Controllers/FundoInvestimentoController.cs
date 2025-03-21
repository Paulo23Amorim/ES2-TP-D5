using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FundoInvestimentoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FundoInvestimentoController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/FundoInvestimento
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FundoInvestimento>>> GetFundosInvestimento()
    {
        return await _context.FundosInvestimento.ToListAsync();
    }

    // GET: api/FundoInvestimento/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<FundoInvestimento>> GetFundoInvestimento(int id)
    {
        var fundoInvestimento = await _context.FundosInvestimento.FindAsync(id);

        if (fundoInvestimento == null)
        {
            return NotFound();
        }

        return fundoInvestimento;
    }
}