using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class DepositoPrazo
{
    [Key]
    public Guid Id { get; set; } 

    [ForeignKey("AtivoFinanceiro")]
    public Guid AtivoId { get; set; } 
    public required AtivoFinanceiro AtivoFinanceiro { get; set; }

    public decimal ValorInicial { get; set; }
    public required string Banco { get; set; }
    public required string NumeroConta { get; set; }
    public required string Titulares { get; set; }
    public decimal TaxaJuroAnual { get; set; }
}
