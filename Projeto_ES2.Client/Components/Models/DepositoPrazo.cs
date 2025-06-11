using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Projeto_ES2.Client.Components.Models;

[Table("DepositosPrazo")]
public class DepositoPrazo
{
    [Key]
    public Guid Id { get; set; } 

    [ForeignKey("AtivoFinanceiro")]
    public Guid AtivoId { get; set; } 
    
    [JsonIgnore]
    public AtivoFinanceiro? AtivoFinanceiro { get; set; }
    public decimal ValorInicial { get; set; }
    public required string Banco { get; set; }
    public required string NumeroConta { get; set; }
    public required string Titulares { get; set; }
    public decimal TaxaJuroAnual { get; set; }
}
