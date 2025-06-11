using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Projeto_ES2.Client.Components.Models;

[Table("ImoveisArrendados")]
public class ImovelArrendado
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("AtivoFinanceiro")]
    public Guid AtivoId { get; set; }
    [JsonIgnore]
    public AtivoFinanceiro? AtivoFinanceiro { get; set; }

    public required string Localizacao { get; set; }
    public required decimal ValorImovel { get; set; }
    public decimal? ValorRenda { get; set; }
    public decimal? ValorCondominio { get; set; }
    public decimal? DespesasAnuais { get; set; }
}