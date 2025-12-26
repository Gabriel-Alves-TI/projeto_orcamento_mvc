using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Services.ItemService;

namespace projeto_orcamento_mvc.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ItemInterface _itemInterface;

        public ItemController(AppDbContext context, ItemInterface itemInterface)
        {
            _context = context;
            _itemInterface = itemInterface;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar(ItemDTO itemDto)
        {
            if (ModelState.IsValid)
            {
                var item = await _itemInterface.AdicionarItem(itemDto);

                if (item.Status)
                {
                    TempData["MensagemSucesso"] = item.Mensagem;
                }

                var itens = await _itemInterface.ListarItens(itemDto.OrcamentoId);

                if (itens.Status)
                {
                    return PartialView("_ListaItens", itens.Dados);
                }
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Editar(ItemDTO itemDto)
        {
            Console.WriteLine("Chegou aqui -----------------------------");
            if (ModelState.IsValid)
            {
                var item = await _itemInterface.EditarItem(itemDto);

                if (item.Status)
                {
                    TempData["MensagemSucesso"] = item.Mensagem;
                }

                var itens = await _itemInterface.ListarItens(itemDto.OrcamentoId);

                if (itens.Status)
                {
                    return PartialView("_ListaItens", itens.Dados);
                }
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(ItemDTO itemDto)
        {
            var item  = await _itemInterface.ExcluirItem(itemDto);

            if (item.Status)
            {
                TempData["MensagemSucesso"] = item.Mensagem;
            }

            var itens = await _itemInterface.ListarItens(itemDto.OrcamentoId);
            Debug.WriteLine(itemDto.OrcamentoId);
            
            if (itens.Status)
            {
                return PartialView("_ListaItens", itens.Dados);
            }

            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}