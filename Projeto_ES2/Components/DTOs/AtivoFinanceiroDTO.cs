namespace Projeto_ES2.Components.DTOs;

public class AtivoFinanceiroDTO
{
    public Guid id { get; set; }
    public string nome { get; set; }
    public string tipo { get; set; }
    public DateTime dataInicio { get; set; }
    public DateTime dataFim { get; set; }
    public decimal imposto { get; set; }
}
