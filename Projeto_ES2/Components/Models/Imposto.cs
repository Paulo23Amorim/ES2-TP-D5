using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class Imposto
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("AtivoFinanceiro")]
    public Guid AtivoId { get; set; }
    public required AtivoFinanceiro AtivoFinanceiro { get; set; }

    public DateTime DataPagamento { get; set; }
    public decimal? ValorPago { get; set; }
}