using System.Net.Http.Json;
using Projeto_ES2.Client.Models;


namespace Projeto_ES2.Client.Services;

public class DashboardService
{
    private readonly HttpClient _http;

    public DashboardService(HttpClient http)
    {
        _http = http;
    }

    public async Task<DashboardStats?> GetEstatisticasAsync()
    {
        return await _http.GetFromJsonAsync<DashboardStats>("api/admin/dashboard-estatisticas");
    }
}

public class DashboardStats
{
    public int TotalUtilizadores { get; set; }
    public int TotalAtivos { get; set; }
    public List<AtivoPorTipo> AtivosPorTipo { get; set; } = new();
}

public class AtivoPorTipo
{
    public string Tipo { get; set; } = "";
    public int Quantidade { get; set; }
}