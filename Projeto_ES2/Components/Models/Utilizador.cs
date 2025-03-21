using System.ComponentModel.DataAnnotations;

namespace Projeto_ES2.Components.Models;

public class Utilizador
{
    [Key]
    public Guid user_id { get; set; }
    
    public String nome { get; set; }
    public String email { get; set; }
    public String password { get; set; }
    
    public TipoUtilizador TipoUtilizador { get; set; }

    public Utilizador(Guid userId, string nome, string email, string password, TipoUtilizador tipoUtilizador)
    {
        user_id = userId;
        this.nome = nome;
        this.email = email;
        this.password = password;
        TipoUtilizador = tipoUtilizador;
    }
} 