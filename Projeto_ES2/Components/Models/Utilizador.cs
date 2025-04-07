using System.ComponentModel.DataAnnotations;

namespace Projeto_ES2.Components.Models;

public class Utilizador
{
    public Guid Id { get; set; }
   
       [Required(ErrorMessage = "O nome é obrigatório.")]
       [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
       public required string Nome { get; set; }
   
       [Required(ErrorMessage = "O e-mail é obrigatório.")]
       [EmailAddress(ErrorMessage = "E-mail inválido.")]
       public required string Email { get; set; }
   
       [Required]
       [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
       public required string Senha { get; set; }

    public TipoUtilizador Tipo { get; set; }

    public List<AtivoFinanceiro> AtivosFinanceiros { get; set; } = new List<AtivoFinanceiro>();
    public List<Invoice> Invoices { get; set; } = new List<Invoice>();
}