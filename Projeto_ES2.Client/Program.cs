using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projeto_ES2.Client;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Projeto_ES2.Client.Services;
using Projeto_ES2.Client.Interceptors;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1) LocalStorage + AuthStateProvider
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthServiceClient>();


// 2) JSON global com enum→string
var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    Converters =
    {
        // converte enum em string e vice‑versa
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
    }
};
builder.Services.AddSingleton(jsonOptions);

// 3) Interceptor de token
builder.Services.AddScoped<AuthTokenInterceptor>();

// 4) HttpClient com interceptor
builder.Services.AddHttpClient("AuthHttpClient", client =>
    {
        client.BaseAddress = new Uri("http://localhost:7010/"); // <- porta do Projeto_ES2.Server
    })
    .AddHttpMessageHandler<AuthTokenInterceptor>();


// 5) Injeta esse client como padrão
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("AuthHttpClient"));

// 6) Serviço HTTP que usa jsonOptions
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<DashboardService>();
;


await builder.Build().RunAsync();