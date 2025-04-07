using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class Invoice
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("Utilizador")]
    public Guid UtilizadorId { get; set; }
    public required Utilizador Utilizador { get; set; }
    public required string Tipo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string? Descricao { get; set; }
}