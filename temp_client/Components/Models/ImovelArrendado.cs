using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Components.Models;

public class ImovelArrendado
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("AtivoFinanceiro")]
    public Guid AtivoId { get; set; }
    public required AtivoFinanceiro AtivoFinanceiro { get; set; }

    public required string Localizacao { get; set; }
    public required decimal ValorImovel { get; set; }
    public decimal? ValorRenda { get; set; }
    public decimal? ValorCondominio { get; set; }
    public decimal? DespesasAnuais { get; set; }
}