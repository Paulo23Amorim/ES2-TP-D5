namespace Projeto_ES2.Components.Pages.DTOs;

public class AtivoFinanceiroDTO
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int Tipo { get; set; } 
    public decimal Imposto { get; set; }
    public Guid UtilizadorId { get; set; }
}