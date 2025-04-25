using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Projeto_ES2.Client.Services;

public class HttpClientService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;

    public HttpClientService(
        HttpClient http,
        ILocalStorageService localStorage,
        NavigationManager navigation)
    {
        _http = http;
        _localStorage = localStorage;
        _navigation = navigation;
    }

    public async Task<HttpResponseMessage> SendAuthorizedRequestAsync(
        HttpMethod method, 
        string uri, 
        object content = null)
    {
        try
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            
            var request = new HttpRequestMessage(method, uri);
            
            if (!string.IsNullOrEmpty(token))
            {
                // Remove aspas que podem existir no token
                token = token.Trim('"');
                request.Headers.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }

            if (content != null)
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(content),
                    Encoding.UTF8,
                    "application/json");
            }

            var response = await _http.SendAsync(request);

            // Log para depuração
            Console.WriteLine($"Requisição {method} para {uri}: Status {response.StatusCode}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/logout");
                return null;
            }

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na requisição HTTP: {ex.Message}");
            throw;
        }
    }
}