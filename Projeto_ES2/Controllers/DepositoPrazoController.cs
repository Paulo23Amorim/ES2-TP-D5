using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Models;

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

    // GET: api/DepositoPrazo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepositoPrazo>>> GetDepositosPrazo()
    {
        return await _context.DepositosPrazo.ToListAsync();
    }

    // GET: api/DepositoPrazo/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<DepositoPrazo>> GetDepositoPrazo(int id)
    {
        var depositoPrazo = await _context.DepositosPrazo.FindAsync(id);

        if (depositoPrazo == null)
        {
            return NotFound();
        }

        return depositoPrazo;
    }
}