using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projeto_ES2.Client;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http;
using Projeto_ES2.Client.Services;
using AuthTokenInterceptor = Projeto_ES2.Client.Interceptors.AuthTokenInterceptor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1) Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();

// 2) CustomAuthStateProvider e AuthorizationCore
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore();

// 3) Registra o interceptor de token (agora sem ambiguidades)
builder.Services.AddScoped<AuthTokenInterceptor>();

// 4) Named client "AuthHttpClient" com o interceptor
builder.Services.AddHttpClient("AuthHttpClient", client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    })
    .AddHttpMessageHandler<AuthTokenInterceptor>();

// 5) Faz do named client o HttpClient padrão (@inject HttpClient Http)
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("AuthHttpClient"));

// 6) Outros serviços da app (HttpClientService, etc.)
builder.Services.AddScoped<HttpClientService>();

await builder.Build().RunAsync();