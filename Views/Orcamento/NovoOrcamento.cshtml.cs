using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace projeto_orcamento_mvc.Views.Orcamento
{
    public class NovoOrcamento : PageModel
    {
        private readonly ILogger<NovoOrcamento> _logger;

        public NovoOrcamento(ILogger<NovoOrcamento> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}