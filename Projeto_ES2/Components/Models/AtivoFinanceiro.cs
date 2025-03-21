using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class AtivoFinanceiro
{
    [Key] // Chave primária
    public Guid Id { get; set; }

    [ForeignKey("Utilizador")]
    public Guid UtilizadorId { get; set; }
    public required Utilizador Utilizador { get; set; }

    public TipoAtivoFinanceiro Tipo { get; set; }

    [Required] 
    [StringLength(100)]
    public required string Nome { get; set; }

    private DateTime _dataInicio;
    private DateTime _dataFim;

    public DateTime DataInicio
    {
        get => _dataInicio;
        set
        {
            if (value > DataFim)
            {
                throw new ArgumentException("DataInicio não pode ser maior que DataFim.");
            }
            _dataInicio = value;
        }
    }

    public DateTime DataFim
    {
        get => _dataFim;
        set
        {
            if (value < DataInicio)
            {
                throw new ArgumentException("DataFim não pode ser menor que DataInicio.");
            }
            _dataFim = value;
        }
    }

    [Range(0, 100)]
    public float Imposto { get; set; }

    public DepositoPrazo? DepositoPrazo { get; set; }
    public FundoInvestimento? FundoInvestimento { get; set; }
    public ImovelArrendado? ImovelArrendado { get; set; }

    public List<Impostos> Impostos { get; set; } = new();
}