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
   public DbSet<DepositoPrazo> DepositosPrazos { get; set; }
   public DbSet<FundoInvestimento> FundosInvestimentos { get; set; }
   public DbSet<Juros> Juros { get; set; }
   public DbSet<ImovelArrendado> ImovelArrendados { get; set; }
   public DbSet<Imposto> Impostos { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuração de chaves estrangeiras e relacionamentos
        modelBuilder.Entity<AtivoFinanceiro>()
            .HasOne(a => a.Utilizador)
            .WithMany(u => u.AtivosFinanceiros)
            .HasForeignKey(a => a.UtilizadorId)
            .OnDelete(DeleteBehavior.Cascade); // Define o comportamento de exclusão

        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Utilizador)
            .WithMany(u => u.Invoices) 
            .HasForeignKey(i => i.UtilizadorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DepositoPrazo>()
            .HasOne(d => d.AtivoFinanceiro)
            .WithOne(a => a.DepositoPrazo)
            .HasForeignKey<DepositoPrazo>(d => d.AtivoId);

        modelBuilder.Entity<FundoInvestimento>()
            .HasOne(f => f.AtivoFinanceiro)
            .WithOne(a => a.FundoInvestimento)
            .HasForeignKey<FundoInvestimento>(f => f.AtivoId);

        modelBuilder.Entity<ImovelArrendado>()
            .HasOne(i => i.AtivoFinanceiro)
            .WithOne(a => a.ImovelArrendado)
            .HasForeignKey<ImovelArrendado>(i => i.AtivoId);

        modelBuilder.Entity<Juros>()
            .HasOne(j => j.FundoInvestimento)
            .WithMany(f => f.Juros) 
            .HasForeignKey(j => j.FundoId);

        modelBuilder.Entity<Imposto>()
            .HasOne(i => i.AtivoFinanceiro)
            .WithMany(a => a.Impostos) 
            .HasForeignKey(i => i.AtivoId);

    }
}