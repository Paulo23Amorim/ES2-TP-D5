using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Client.Components.Models;

[Table("FundosInvestimento")]
public class FundoInvestimento
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("AtivoFinanceiro")]
    public Guid AtivoId { get; set; }
    public required AtivoFinanceiro AtivoFinanceiro { get; set; }

    public decimal MontanteInvestido { get; set; }
    public decimal TaxaJuroPadrao { get; set; }
    
    public List<Juros> Juros { get; set; } = new();
}
