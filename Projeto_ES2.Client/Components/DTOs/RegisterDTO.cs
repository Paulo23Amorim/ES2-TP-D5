using System.ComponentModel.DataAnnotations;
using Projeto_ES2.Client.Components.Models;

namespace Projeto_ES2.Client.Components.DTOs;

public class RegisterDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public TipoUtilizador Tipo { get; set; } = TipoUtilizador.Utilizador; 
}