using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Client.Components.Models;

public class Utilizador
{
    [Key]
    public Guid user_id { get; set; }
    
    public String nome { get; set; } = string.Empty;
    public String email { get; set; } = string.Empty;
    
    [Column("password")]
    public string PasswordHash { get; set; } = string.Empty;

    
    public TipoUtilizador TipoUtilizador { get; set; }
    
    public Utilizador() { }

    public Utilizador(Guid userId, string nome, string email, string password, TipoUtilizador tipoUtilizador)
    {
        user_id = userId;
        this.nome = nome;
        this.email = email;
        this.PasswordHash = password;
        TipoUtilizador = tipoUtilizador;
    }
    
    public List<AtivoFinanceiro> AtivosFinanceiros { get; set; } = new List<AtivoFinanceiro>();
    public List<Invoice> Invoices { get; set; } = new List<Invoice>();
}