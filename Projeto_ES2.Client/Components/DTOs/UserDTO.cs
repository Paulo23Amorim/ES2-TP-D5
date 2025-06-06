using System.Text.Json;
using Projeto_ES2.Client.Components.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Projeto_ES2.Client.Components.DTOs;

public class UserDTO
{
    public Guid user_id { get; set; }
    public string nome { get; set; }
    public string email { get; set; }

    public int Tipo { get; set; }

}