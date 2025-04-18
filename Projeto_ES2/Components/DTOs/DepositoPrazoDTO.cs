using System.Text.Json.Serialization;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Components.Pages.DTOs;

public class DepositoPrazoDto
{
    public Guid Id { get; set; }
    public decimal ValorInicial { get; set; }
    public string NumeroConta { get; set; }
    public string Titulares { get; set; }
    public decimal TaxaJuroAnual { get; set; }
    public string Banco { get; set; }

    [JsonInclude] 
    public AtivoFinanceiro? AtivoFinanceiro { get; set; }
}