using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Components.DTOs;

public class AtivoFinanceiroNovoDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Nome { get; set; } = string.Empty;
    public TipoAtivoFinanceiro Tipo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public decimal Imposto { get; set; }

    public DepositoPrazoDTO? Deposito { get; set; }
    public FundoInvestimentoDTO? Fundo { get; set; }
    public ImovelArrendadoDTO? Imovel { get; set; }
}

public class DepositoPrazoDTO
{
    public decimal ValorInicial { get; set; }
    public string Banco { get; set; } = string.Empty;
    public string NumeroConta { get; set; } = string.Empty;
    public string Titulares { get; set; } = string.Empty; 
    public decimal TaxaJuroAnual { get; set; }
}

public class FundoInvestimentoDTO
{
    public decimal Montante { get; set; }
    public decimal TaxaJuroPadrao { get; set; }
}

public class ImovelArrendadoDTO
{
    public string Localizacao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public decimal Renda { get; set; }
    public decimal Condominio { get; set; }
    public decimal Despesas { get; set; }
}

