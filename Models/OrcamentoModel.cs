using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace projeto_orcamento_mvc.Models
{
    public class OrcamentoModel
    {
        public int Id { get; set; }
        public int Numeracao { get; set; }
        public DateTime Data { get; set; }
        public int ClienteId { get; set; }
        [ValidateNever]
        public ClienteModel Cliente { get; set; }
        public int ValidadeProposta { get; set; }
        public int Previsao { get; set; }
        public List<ItemModel>? Itens { get; set; } = new List<ItemModel>();
        public decimal ValorTotal
        {
            get
            {
                if (Itens == null)
                    return 0;

                return Itens.Sum(item => item.ValorTotal);
            }
        }
    }
}