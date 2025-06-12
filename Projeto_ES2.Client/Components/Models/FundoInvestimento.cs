using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Projeto_ES2.Client.Components.Models;

[Table("FundosInvestimento")]
public class FundoInvestimento
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }  


    [ForeignKey("AtivoFinanceiro")]
    public Guid AtivoId { get; set; }
    [JsonIgnore]
    public AtivoFinanceiro? AtivoFinanceiro { get; set; }
    public decimal MontanteInvestido { get; set; }
    public decimal TaxaJuroPadrao { get; set; }
    
    public List<Juros> Juros { get; set; } = new();
}
