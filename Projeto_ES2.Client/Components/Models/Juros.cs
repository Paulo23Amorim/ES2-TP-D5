using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Client.Components.Models;

public class Juros
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("FundoInvestimento")]
    public Guid FundoId { get; set; }
    public required FundoInvestimento FundoInvestimento { get; set; }

    public int MesReferencia { get; set; }
    public decimal TaxaJuro { get; set; }
}