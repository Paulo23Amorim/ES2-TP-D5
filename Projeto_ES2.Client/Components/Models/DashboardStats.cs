namespace Projeto_ES2.Client.Models;

public class DashboardStats
{
    public int TotalUtilizadores { get; set; }
    public int TotalClientes { get; set; }
    public int TotalAtivos { get; set; }
    public List<AtivoPorTipo> AtivosPorTipo { get; set; } = new();
}


public class AtivoPorTipo
{
    public string Tipo { get; set; } = "";
    public int Quantidade { get; set; }
}

public class AtivoResumo
{
    public string Nome { get; set; } = "";
    public string Tipo { get; set; } = "";
    public decimal ValorAtual { get; set; }
}