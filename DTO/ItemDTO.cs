using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.DTO
{
    public class ItemDTO
    {
        public int? Id { get; set; }
        public string Descricao { get; set; }
        public decimal ValorMaterial { get; set; }
        public decimal ValorServico { get; set; }
        public decimal ValorTotal => ValorMaterial + ValorServico;
        public int OrcamentoId { get; set; }
        [ValidateNever] // Ignora para o EF e o model binder
        public OrcamentoModel Orcamento { get; set; }
    }
}