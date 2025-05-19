using System.Text.Json.Serialization;

namespace Projeto_ES2.Client.Components.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoAtivoFinanceiro
    {
        DepositoPrazo = 0,
        FundoInvestimento = 1,
        ImovelArrendado = 2
    }
}
