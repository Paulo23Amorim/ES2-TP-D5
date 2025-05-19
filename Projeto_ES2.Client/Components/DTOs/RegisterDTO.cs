using System.ComponentModel.DataAnnotations;
using Projeto_ES2.Client.Components.Models;

namespace Projeto_ES2.Client.Components.DTOs;

public class RegisterDTO
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password é obrigatória")]
    [MinLength(6, ErrorMessage = "Password deve ter no mínimo 6 caracteres")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Tipo de utilizador é obrigatório")]
    public TipoUtilizador Tipo { get; set; } = TipoUtilizador.Utilizador; // Valor padrão
}