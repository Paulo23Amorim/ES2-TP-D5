using Projeto_ES2.Client.Components.Models;

namespace Projeto_ES2.Server.Services;

public class ImpostoFundoInvestimento : ImpostoStrategyAtivos
{
    public decimal CalcularImposto(AtivoFinanceiro ativo)
    {
        var fundo = ativo.FundoInvestimento;
        if (fundo == null) return 0;

        decimal lucro = fundo.MontanteInvestido * (fundo.TaxaJuroPadrao / 100);
        return lucro > 0 ? lucro * (ativo.Imposto / 100) : 0;
    }
}