using Projeto_ES2.Client.Components.Models;

namespace Projeto_ES2.Server.Services;

public class ImpostoImovelArrendado : ImpostoStrategyAtivos
{
    public decimal CalcularImposto(AtivoFinanceiro ativo)
    {
        var imovel = ativo.ImovelArrendado;
        if (imovel == null || !imovel.ValorRenda.HasValue) return 0;

        decimal receitaAnual = imovel.ValorRenda.Value * 12;
        decimal despesas = (imovel.ValorCondominio ?? 0) * 12 + (imovel.DespesasAnuais ?? 0);
        decimal lucro = receitaAnual - despesas;

        return lucro > 0 ? lucro * (ativo.Imposto / 100) : 0;
    }
}