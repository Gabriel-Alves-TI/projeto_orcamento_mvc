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
using projeto_orcamento_mvc.Services.ClienteService;
using projeto_orcamento_mvc.ViewModel;

namespace projeto_orcamento_mvc.Controllers
{
    public class ClienteController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IClienteInterface _clienteInterface;

        public ClienteController(AppDbContext context, IClienteInterface clienteInterface)
        {
            _context = context;
            _clienteInterface = clienteInterface;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteInterface.BuscarClientes();

            var viewModel = new ClientesViewModel
            {
                Clientes = clientes.Dados,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Cadastrar()
        {
            var clienteDto = new ClienteDTO();

            return View(clienteDto);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(ClienteDTO clienteDto)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Está vindo para cá");
                // ModelState.AddModelError("", "CPF/CNPJ é obrigatório");
                return View(clienteDto);
            }

            if (ModelState.IsValid)
            {
                var cliente = await _clienteInterface.CadastrarCliente(clienteDto);

                if (cliente.Status)
                {
                    TempData["MensagemSucesso"] = cliente.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = cliente.Mensagem;
                    return View(clienteDto);
                }

                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Ocorreu algum erro ao salvar o Cliente.";
            return RedirectToAction("Index");   
        }

        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null)
            {
                TempData["MensagemErro"] = "Cliente não localizado";
                return RedirectToAction("Index");
            }

            var clienteDto = new ClienteDTO
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                CpfCnpj = cliente.CpfCnpj,
                Cep = cliente.Cep,
                Logradouro = cliente.Logradouro,
                Numero = cliente.Numero,
                Cidade = cliente.Cidade,
                Estado = cliente.Estado,
                Telefone = cliente.Telefone
            };

            return View(clienteDto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(ClienteDTO clienteDto)
        {
            if (ModelState.IsValid)
            {
                var cliente = await _clienteInterface.EditarCliente(clienteDto);

                if (cliente.Status)
                {
                    TempData["MensagemSucesso"] = cliente.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = cliente.Mensagem;
                    return View(clienteDto);
                }

                return RedirectToAction("Index");
            }
            TempData["MensagemErro"] = "Ocorreu algum erro ao editar o Cliente.";
            return View(clienteDto);   
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null)
            {
                TempData["MensagemErro"] = "Cliente não localizado";
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"ClienteId recebido: {id}");
                var cliente = await _clienteInterface.ExcluirCliente(id);

                if (cliente.Status)
                {
                    TempData["MensagemSucesso"] = cliente.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = cliente.Mensagem;
                    return View(cliente);
                }

                return RedirectToAction("Index");
            }

            TempData["MensagemErro"] = "Ocorreu algum erro ao excluir o Cliente.";
            return RedirectToAction("Index");   
        }

        [HttpGet]
        public IActionResult ListarClientes()
        {
            var clientes = _context.Clientes
                .Select(c => new {c.Id, c.Nome})
                .OrderBy(c => c.Nome)
                .ToList();
            
            return Json(clientes);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarClientesInput(string nome)
        {
            var clientes = await _context.Clientes
                .Where(c => c.Nome.Contains(nome))
                .Select(c => new {c.Id , c.Nome})
                .ToListAsync();
            
            return Json(clientes);
        }

        public async Task<IActionResult> DadosCompletosCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null) return NotFound();
            
            return Json(new
            {
                cliente.CpfCnpj,
                cliente.Telefone,
                cliente.Logradouro,
                cliente.Cep,
                cliente.Numero,
                cliente.Estado,
                cliente.Cidade
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}