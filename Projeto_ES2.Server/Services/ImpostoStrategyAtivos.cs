using Projeto_ES2.Client.Components.Models;

namespace Projeto_ES2.Server.Services;

public interface ImpostoStrategyAtivos
{
    decimal CalcularImposto(AtivoFinanceiro ativo);
}


