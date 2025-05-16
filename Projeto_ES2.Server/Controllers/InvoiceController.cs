using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Client.Components.Models;
using Projeto_ES2.Server.Data;

namespace Projeto_ES2.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InvoiceController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Invoice
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
    {
        return await _context.Invoices.Include(i => i.Utilizador).ToListAsync();
    }

    // GET: api/Invoice/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Utilizador)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
        {
            return NotFound();
        }

        return invoice;
    }

    // POST: api/Invoice
    [HttpPost]
    public async Task<ActionResult<Invoice>> CreateInvoice(Invoice invoice)
    {
        invoice.Id = Guid.NewGuid();
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
    }

    // PUT: api/Invoice/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(Guid id, Invoice updatedInvoice)
    {
        if (id != updatedInvoice.Id)
        {
            return BadRequest("O ID da URL não corresponde ao ID do corpo da requisição.");
        }

        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        // Atualiza os campos necessários
        invoice.UtilizadorId = updatedInvoice.UtilizadorId;
        invoice.Tipo = updatedInvoice.Tipo;
        invoice.DataInicio = updatedInvoice.DataInicio;
        invoice.DataFim = updatedInvoice.DataFim;
        invoice.Descricao = updatedInvoice.Descricao;

        _context.Entry(invoice).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Invoice/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
