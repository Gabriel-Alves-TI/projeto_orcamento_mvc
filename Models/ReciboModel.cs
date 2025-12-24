using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace projeto_orcamento_mvc.Models
{
    [Table("Recibos")]
    public class ReciboModel
    {
        public int Id { get; set; }
        public int Numeracao { get; set; }
        public DateTime Data { get; set; }
        public int ClienteId { get; set; }
        [ValidateNever]
        public ClienteModel Cliente { get; set; }
        public string? Descricao { get; set; }
        public decimal ValorRecibo { get; set; }
        public DateTime? Vencimento { get; set; }
        public int? NumNotaFiscal { get; set; }
    }
}