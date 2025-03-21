using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class FundoInvestimento
{
    [Key]
    public Guid id { get; set; }

    [ForeignKey("AtivoFinanceiro")]
    public Guid ativo_id { get; set; }
    public required AtivoFinanceiro AtivoFinanceiro { get; set; }

    public decimal montante_investido { get; set; }
    public float taxa_juro_padrao { get; set; }
    
    public List<Juros> Juros { get; set; } = new();
}