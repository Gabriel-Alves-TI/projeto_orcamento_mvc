using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projeto_orcamento_mvc.Models
{
    public class HomeIndexView
    {
        public List<OrcamentoModel> Orcamentos { get; set; }
        public List<ReciboModel> Recibos { get; set; }
    }
}