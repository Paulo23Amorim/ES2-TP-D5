using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Projeto_ES2.Client.Components.DTOs;

namespace Projeto_ES2.Client.Services;

public class AuthServiceClient
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly CustomAuthStateProvider _authStateProvider;

    public AuthServiceClient(
        HttpClient http,
        ILocalStorageService localStorage,
        AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _localStorage = localStorage;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new LoginRequestDTO
        {
            Email = email,
            Password = password
        });

        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<LoginResultDTO>();
        
        if (result == null)
            return false;

        await _localStorage.SetItemAsync("authToken", result.Token);
        _authStateProvider.NotifyUserAuthentication(result.Token);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
        
        return true;
    }

    public async Task<bool> RegisterAsync(RegisterDTO registerDto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", registerDto);
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _authStateProvider.NotifyUserLogout();
        _http.DefaultRequestHeaders.Authorization = null;
    }
}