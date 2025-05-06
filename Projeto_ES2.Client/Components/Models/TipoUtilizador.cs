using System.Text.Json.Serialization;

namespace Projeto_ES2.Client.Components.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoUtilizador
    {
        Utilizador = 0,
        UserManager = 1,
        Admin = 2
    }
}