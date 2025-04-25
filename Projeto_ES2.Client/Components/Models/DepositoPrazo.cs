using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Client.Components.Models;

public class DepositoPrazo
{
    [Key]
    public Guid Id { get; set; } //PascalCase id -> Id

    [ForeignKey("AtivoFinanceiro")]
    public Guid AtivoId { get; set; } //ativo_id -> AtivoId
    public required AtivoFinanceiro AtivoFinanceiro { get; set; }

    public decimal ValorInicial { get; set; }
    public required string Banco { get; set; }
    public required string NumeroConta { get; set; }
    public required string Titulares { get; set; }
    public decimal TaxaJuroAnual { get; set; }
}
