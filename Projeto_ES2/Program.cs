using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Projeto_ES2.Components;
using Projeto_ES2.Components.Data;
using Projeto_ES2.Components.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adicionar controllers de API
builder.Services.AddControllers();

// Adicionar serviços do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gestor de Ativos Financeiros",
        Version = "v1",
        Description = "API para a aplicação Blazor Sandbox"
    });
});

builder.Services.AddHttpClient();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44369/") });

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Mapear controllers
app.MapControllers();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();