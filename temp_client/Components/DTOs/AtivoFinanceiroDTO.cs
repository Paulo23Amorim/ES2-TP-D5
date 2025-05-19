using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Components.DTOs;

public class AtivoFinanceiroDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public TipoAtivoFinanceiro Tipo { get; set; } // Deve ser do tipo enum, não string
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    
    public decimal Imposto { get; set; } // Adicione esta linha

    public DepositoPrazoDTO? Deposito { get; set; }
    public FundoInvestimentoDTO? Fundo { get; set; }
    public ImovelArrendadoDTO? Imovel { get; set; }
}
