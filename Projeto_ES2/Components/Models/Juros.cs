using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class Juros
{
    [Key]
    public Guid id { get; set; }

    [ForeignKey("FundoInvestimento")]
    public Guid fundo_id { get; set; }
    public required FundoInvestimento FundoInvestimento { get; set; }

    public int mes_referencia { get; set; }
    public float taxa_juro { get; set; }
}