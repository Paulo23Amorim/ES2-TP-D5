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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurações adicionais do modelo podem ser adicionadas aqui
    }
}