using Projeto_ES2.Client.Components.Models;

namespace Projeto_ES2.Server.Services;

public class ImpostoDepositoPrazo : ImpostoStrategyAtivos
{
    public decimal CalcularImposto(AtivoFinanceiro ativo)
    {
        var deposito = ativo.DepositoPrazo;
        if (deposito == null) return 0;

        decimal lucro = deposito.ValorInicial * (deposito.TaxaJuroAnual / 100);
        return lucro > 0 ? lucro * (ativo.Imposto / 100) : 0;
    }
}