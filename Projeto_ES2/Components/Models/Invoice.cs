using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class Invoice
{
    [Key]
    public Guid id { get; set; }

    [ForeignKey("Utilizador")]
    public Guid utilizador_id { get; set; }
    public required Utilizador Utilizador { get; set; }

    public required string tipo { get; set; }
    public DateTime data_inicio { get; set; }
    public DateTime data_fim { get; set; }
    public string? descricao { get; set; }
}