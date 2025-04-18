using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class AtivoFinanceiro
{
    [Key] 
    public Guid Id { get; set; }

    [ForeignKey("Utilizador")]
    public Guid UtilizadorId { get; set; }
    public Utilizador Utilizador { get; set; }

    public TipoAtivoFinanceiro Tipo { get; set; }

    [Required] 
    [StringLength(100)]
    public required string Nome { get; set; }
    
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }

    [Range(0, 100)]
    public decimal Imposto { get; set; }

    public DepositoPrazo? DepositoPrazo { get; set; }
    public FundoInvestimento? FundoInvestimento { get; set; }
    public ImovelArrendado? ImovelArrendado { get; set; }

    public List<Imposto> Impostos { get; set; } = new();
}
