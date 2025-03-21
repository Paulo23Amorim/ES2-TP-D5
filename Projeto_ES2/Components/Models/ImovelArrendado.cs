using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class ImovelArrendado
{
    [Key]
    public Guid id { get; set; }

    [ForeignKey("AtivoFinanceiro")]
    public Guid ativo_id { get; set; }
    public required AtivoFinanceiro AtivoFinanceiro { get; set; }

    public required string localizacao { get; set; }
    public decimal valor_imovel { get; set; }
    public decimal valor_renda { get; set; }
    public decimal valor_condominio { get; set; }
    public decimal despesas_anuais { get; set; }
}