using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Projeto_ES2.Components;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração de logging para capturar logs detalhados
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Adiciona o log no console
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Define o nível mínimo de log

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configuração do DbContext com PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configuração dos controllers
builder.Services.AddControllers();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sandbox API",
        Version = "v1",
        Description = "API para a aplicação Blazor Sandbox"
    });
});

// Adicionar HttpClient
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44369/") });


//Services
builder.Services.AddScoped<AuthService>();


var app = builder.Build();

// Configuração da pipeline de requisições
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sandbox API V1");
    });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Mapear controllers
app.MapControllers();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseDeveloperExceptionPage(); // Garante que erros são expostos durante o desenvolvimento

app.Run();
