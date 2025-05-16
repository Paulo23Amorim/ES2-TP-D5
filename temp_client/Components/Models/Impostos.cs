using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_ES2.Components.Models;

public class Impostos
{
    [Key]
    public Guid id { get; set; }

    [ForeignKey("AtivoFinanceiro")]
    public Guid ativo_id { get; set; }
    public required AtivoFinanceiro AtivoFinanceiro { get; set; }

    public DateTime data_pagamento { get; set; }
    public decimal valor_pago { get; set; }
}