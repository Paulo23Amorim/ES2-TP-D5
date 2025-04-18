namespace Projeto_ES2.Components.Pages.DTOs;

public class DepositoPrazoCreateDto
{
    public string Banco { get; set; }
    public string NumeroConta { get; set; }
    public decimal TaxaJuroAnual { get; set; }
    public string Titulares { get; set; }
    public decimal ValorInicial { get; set; }

    public AtivoFinanceiroDTO AtivoFinanceiro { get; set; } = null!;
}

