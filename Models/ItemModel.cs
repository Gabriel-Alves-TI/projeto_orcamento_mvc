using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace projeto_orcamento_mvc.Models
{
    [Table("Itens")]
    public class ItemModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal ValorMaterial { get; set; }
        public decimal ValorServico { get; set; }
        public decimal ValorTotal => ValorMaterial + ValorServico;
        public int OrcamentoId { get; set; }
        [ValidateNever] // Ignora para o EF e o model binder
        public OrcamentoModel Orcamento { get; set; }
    }
}