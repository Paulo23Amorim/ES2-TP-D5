using Microsoft.EntityFrameworkCore;
using Projeto_ES2.Components.Models;

namespace Projeto_ES2.Components.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Utilizador> Utilizadores { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<AtivoFinanceiro> AtivosFinanceiros { get; set; }
    public DbSet<DepositoPrazo> DepositosPrazo { get; set; }
    public DbSet<FundoInvestimento> FundosInvestimento { get; set; }
    public DbSet<Juros> Juros { get; set; }
    public DbSet<ImovelArrendado> ImoveisArrendados { get; set; }
    public DbSet<Impostos> Impostos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurações adicionais do modelo podem ser adicionadas aqui
    }
}