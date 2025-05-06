using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Projeto_ES2.Client.Components.DTOs;

namespace Projeto_ES2.Client.Services
{
    public class HttpClientService
    {
        private readonly HttpClient            _http;
        private readonly ILocalStorageService  _localStorage;
        private readonly NavigationManager     _navigation;
        private readonly JsonSerializerOptions _jsonOptions;

        public HttpClientService(
            HttpClient http,
            ILocalStorageService localStorage,
            NavigationManager navigation,
            JsonSerializerOptions jsonOptions)
        {
            _http         = http;
            _localStorage = localStorage;
            _navigation   = navigation;
            _jsonOptions  = jsonOptions;
        }

        /// <summary>
        /// Exemplo de método GET que já utiliza as opções de JSON (enum→string).
        /// </summary>
        public Task<List<AtivoFinanceiroDTO>?> GetAtivosAsync() =>
            _http.GetFromJsonAsync<List<AtivoFinanceiroDTO>>(
                "api/AtivoFinanceiro", _jsonOptions);

        /// <summary>
        /// Envia requisições autenticadas (POST, PUT, DELETE…).
        /// </summary>
        public async Task<HttpResponseMessage?> SendAuthorizedRequestAsync(
            HttpMethod method,
            string uri,
            object? content = null)
        {
            var token = (await _localStorage.GetItemAsStringAsync("authToken"))
                            ?.Trim('"');

            var req = new HttpRequestMessage(method, uri);
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            if (content is not null)
                req.Content = new StringContent(
                    JsonSerializer.Serialize(content, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

            var resp = await _http.SendAsync(req);
            Console.WriteLine($"[HTTP] {method} {uri} → {resp.StatusCode}");

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/logout");
                return null;
            }

            return resp;
        }
    }
}
