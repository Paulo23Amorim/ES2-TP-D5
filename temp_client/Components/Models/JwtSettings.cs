namespace Projeto_ES2.Components.Models;

public class JwtSettings
{
    public string Key { get; set; } // Chave secreta para assinar o token
    public string Issuer { get; set; } // Quem emite o token (seu servidor)
    public string Audience { get; set; } // Para quem o token é destinado (sua aplicação)
    public int ExpiryInMinutes { get; set; } // Tempo de validade do token
}