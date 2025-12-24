using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace projeto_orcamento_mvc.Views.Shared
{
    public class _ModalBuscarCliente : PageModel
    {
        private readonly ILogger<_ModalBuscarCliente> _logger;

        public _ModalBuscarCliente(ILogger<_ModalBuscarCliente> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}