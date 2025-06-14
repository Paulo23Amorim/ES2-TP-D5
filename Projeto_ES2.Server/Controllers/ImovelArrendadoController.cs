using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.DTOs;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;

namespace Projeto_ES2.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImovelArrendadoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ImovelArrendadoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ImovelArrendado>>> GetAll()
    {
        return await _context.ImovelArrendados
            .Include(i => i.AtivoFinanceiro)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ImovelArrendado>> GetById(Guid id)
    {
        var imovel = await _context.ImovelArrendados
            .Include(i => i.AtivoFinanceiro)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (imovel == null) return NotFound();

        return imovel;
    }

    [HttpPost]
    public async Task<ActionResult<ImovelArrendado>> Create([FromBody] ImovelArrendadoDTO imovelDto)
    {
        // Validação do modelo
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Criação do AtivoFinanceiro
        var ativoFinanceiro = new AtivoFinanceiro
        {
            Id = Guid.NewGuid(), // Gera novo ID
            Nome = "Imóvel - " + imovelDto.Localizacao,
            Tipo = TipoAtivoFinanceiro.ImovelArrendado,
            DataInicio = DateTime.Now,
            Imposto = 0.15m
        };

        // Criação do Imóvel
        var imovel = new ImovelArrendado
        {
            Id = Guid.NewGuid(), // Gera novo ID
            AtivoFinanceiro = ativoFinanceiro,
            Localizacao = imovelDto.Localizacao,
            ValorImovel = imovelDto.ValorImovel,
            ValorRenda = imovelDto.ValorRenda,
            ValorCondominio = imovelDto.Condominio,
            DespesasAnuais = imovelDto.Despesas
        };

        _context.ImovelArrendados.Add(imovel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = imovel.Id }, imovel);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ImovelArrendadoDTO imovelDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var imovel = await _context.ImovelArrendados
            .Include(i => i.AtivoFinanceiro)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (imovel == null) return NotFound();

        // Mapeamento correto das propriedades
        imovel.Localizacao = imovelDto.Localizacao;
        imovel.ValorImovel = imovelDto.ValorImovel;
    
        // Mapeamento dos campos nullable com nomes diferentes
        imovel.ValorRenda = imovelDto.ValorRenda;
        imovel.ValorCondominio = imovelDto.Condominio;
        imovel.DespesasAnuais = imovelDto.Despesas;

        // Atualização do nome no AtivoFinanceiro
        if (imovel.AtivoFinanceiro != null)
        {
            imovel.AtivoFinanceiro.Nome = "Imóvel - " + imovelDto.Localizacao;
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.ImovelArrendados.Any(i => i.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var imovel = await _context.ImovelArrendados.FindAsync(id);
        if (imovel == null) return NotFound();

        _context.ImovelArrendados.Remove(imovel);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}