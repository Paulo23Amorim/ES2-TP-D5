using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class DepositoPrazo
{
    [Key]
    public Guid id { get; set; }

    [ForeignKey("AtivoFinanceiro")]
    public Guid ativo_id { get; set; }
    public required AtivoFinanceiro AtivoFinanceiro { get; set; }

    public decimal valor_inicial { get; set; }
    public required string banco { get; set; }
    public required string numero_conta { get; set; }
    public required string titulares { get; set; }
    public float taxa_juro_anual { get; set; }
}