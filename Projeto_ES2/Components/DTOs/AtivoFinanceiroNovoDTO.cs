using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Components.DTOs;

public class AtivoFinanceiroNovoDTO
{
    public string Nome { get; set; } = string.Empty;
    public TipoAtivoFinanceiro Tipo { get; set; }
    public DateTime DataInicio { get; set; }
    public int DuracaoMeses { get; set; }
    public decimal Imposto { get; set; }

    public DepositoPrazoDTO? Deposito { get; set; }
    public FundoInvestimentoDTO? Fundo { get; set; }
    public ImovelArrendadoDTO? Imovel { get; set; }
}

public class DepositoPrazoDTO
{
    public decimal Valor { get; set; }
    public string Banco { get; set; } = string.Empty;
    public string NumeroConta { get; set; } = string.Empty;
    public string Titulares { get; set; } = string.Empty; // Adicionado!
    public decimal TaxaJuro { get; set; }
}

public class FundoInvestimentoDTO
{
    public decimal Montante { get; set; }
    public decimal TaxaJuroInicial { get; set; }
}

public class ImovelArrendadoDTO
{
    public string Designacao { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public decimal Renda { get; set; }
    public decimal Condominio { get; set; }
    public decimal Despesas { get; set; }
}
