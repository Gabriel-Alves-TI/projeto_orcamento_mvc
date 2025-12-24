using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace projeto_orcamento_mvc.Views.Shared
{
    public class _EditarItem : PageModel
    {
        private readonly ILogger<_EditarItem> _logger;

        public _EditarItem(ILogger<_EditarItem> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}