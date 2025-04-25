using Projeto_ES2.Client.Components.Models;

namespace Projeto_ES2.Server.Services;

public class CalculadoraImposto
{
    private ImpostoStrategyAtivos _strategy;

    public void DefinirEstrategia(ImpostoStrategyAtivos strategy)
    {
        _strategy = strategy;
    }

    public decimal Calcular(AtivoFinanceiro ativo)
    {
        return _strategy.CalcularImposto(ativo);
    }
}