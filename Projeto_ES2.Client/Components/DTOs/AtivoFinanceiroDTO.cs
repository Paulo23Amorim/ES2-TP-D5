using Projeto_ES2.Client.Components.Models;

namespace Projeto_ES2.Client.Components.DTOs;

public class AtivoFinanceiroDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public TipoAtivoFinanceiro Tipo { get; set; } // Deve ser do tipo enum, não string
    public DateTime DataInicio { get; set; } = DateTime.UtcNow; // Valor padrão UTC

    public DateTime? DataFim { get; set; } = DateTime.UtcNow; // Valor padrão UTC
    
    public decimal Imposto { get; set; } // Adicione esta linha
    
    public Utilizador? Utilizador { get; set; }
    public DepositoPrazoDTO? DepositoPrazo { get; set; }
    public FundoInvestimentoDTO? FundoInvestimento { get; set; }
    public ImovelArrendadoDTO? ImovelArrendado { get; set; }
}
