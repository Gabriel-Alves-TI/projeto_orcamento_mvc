using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace projeto_orcamento_mvc.Views.Orcamento.Itens
{
    public class _ListaItens : PageModel
    {
        private readonly ILogger<_ListaItens> _logger;

        public _ListaItens(ILogger<_ListaItens> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}