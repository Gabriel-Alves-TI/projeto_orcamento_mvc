using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.Models;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Services.ReciboService;
using Microsoft.EntityFrameworkCore;
using FastReport.Export.PdfSimple;

namespace projeto_orcamento_mvc.Controllers
{
    public class ReciboController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IReciboInterface _reciboInterface;
        private readonly IWebHostEnvironment _webHostEnv;
        public ReciboController(AppDbContext context, IReciboInterface reciboInterface, IWebHostEnvironment webHostEnv)
        {
            _context = context;
            _reciboInterface = reciboInterface;
            _webHostEnv = webHostEnv;
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            int proximaNumeracao = 1;

            var ultimoRecibo = _context.Recibos
                .OrderByDescending(rec => rec.Numeracao)
                .FirstOrDefault();

            if (ultimoRecibo != null)
            {
                proximaNumeracao = ultimoRecibo.Numeracao + 1;
            }

            var novoRecibo = new ReciboDTO
            {
                Numeracao = proximaNumeracao
            };

            ViewBag.datenow = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));  
            return View(novoRecibo);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(ReciboDTO reciboDto)
        {
            if (!ModelState.IsValid)
            {
                if (reciboDto == null)
                {
                    Console.WriteLine("CPF está vindo nulo");
                    TempData["MensagemErro"] = "Ocorreu um erro ao tentar salvar Recibo.";
                    return View();
                }

                return View(reciboDto);
            }
            
            if (ModelState.IsValid)
            {
                var cliente = _context.Clientes.FirstOrDefault(c => c.Nome == reciboDto.Cliente);

                if (cliente == null)
                {
                    TempData["MensagemErro"] = "Cliente não encontrado. Cadastre o cliente para prosseguir!";
                    return RedirectToAction("Index", "Cliente");
                }

                var recibo = await _reciboInterface.CriarRecibo(reciboDto);

                if (recibo.Status)
                {
                    TempData["MensagemSucesso"] = recibo.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = recibo.Mensagem;
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["MensagemErro"] = "Ocorreu algum erro ao salvar o Orçamento.";
            return View(reciboDto);
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var recibo = _context.Recibos
                .Include(r => r.Cliente)
                .FirstOrDefault(r => r.Id == id);

            if (recibo == null)
            {
                TempData["MensagemErro"] = "Recibo não localizado";
                return RedirectToAction("Index", "Home");
            }

            var reciboDTO = new ReciboDTO
            {
                Id = recibo.Id,
                Numeracao = recibo.Numeracao,
                Data = recibo.Data,
                Cliente = recibo.Cliente.Nome,
                ClienteId = recibo.ClienteId,
                //Dados Cliente
                CpfCnpj = recibo.Cliente.CpfCnpj,
                Cep = recibo.Cliente.Cep.ToString(),
                Logradouro = recibo.Cliente.Logradouro,
                Numero = recibo.Cliente.Numero.ToString(),
                Cidade = recibo.Cliente.Cidade,
                Estado = recibo.Cliente.Estado,
                Telefone = recibo.Cliente.Telefone,

                Descricao = recibo.Descricao,
                ValorRecibo = recibo.ValorRecibo,
                Vencimento = recibo.Vencimento,
                NumNotaFiscal = recibo.NumNotaFiscal
            };

            return View(reciboDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(ReciboDTO reciboDto)
        {
            var recibo = await _reciboInterface.EditarRecibo(reciboDto);
            if (recibo.Status)
            {
                TempData["MensagemSucesso"] = recibo.Mensagem;
            }
            else
            {
                TempData["MensagemErro"] = recibo.Mensagem;
                return View(reciboDto);
            }   

            TempData["MensagemSucesso"] = recibo.Mensagem;
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Excluir(int id)
        {
            var recibo = _context.Recibos
                .Include(r => r.Cliente)
                .FirstOrDefault(r => r.Id == id);
            if (recibo == null)
            {
                TempData["MensagemErro"] = "Recibo não localizado";
                return RedirectToAction("Index", "Home");
            }

            return View(recibo);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var recibo = await _reciboInterface.ExcluirRecibo(id);
            if (recibo.Status)
            {
                TempData["MensagemSucesso"] = recibo.Mensagem;
            }
            else
            {
                TempData["MensagemErro"] = recibo.Mensagem;
                return RedirectToAction("Index", "Home");
            }

            TempData["MensagemSucesso"] = "Recibo excluído com sucesso";
            return RedirectToAction("Index", "Home");
        }

        // MÉTODO PARA GERAR ARQUIVO .FRX

        // public async Task<IActionResult> CreateReport(int id)
        // {
        //     var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"reports/Recibo.frx");
        //     var reportFile = caminhoReport;

        //     var freport = new FastReport.Report();

        //     var resultado = await _reciboInterface.BuscarReciboPorId(id);

        //     freport.Dictionary.RegisterBusinessObject(new List<ReciboModel> {resultado.Dados}, "Recibo", 10, true);
        //     freport.Report.Save(reportFile);

        //     return Ok($"Relatório gerado: {caminhoReport}");
        // }

        public async Task<IActionResult> ReciboRelatorio(int id)
        {
            var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"reports/Recibo.frx");
            var reportFile = caminhoReport;

            var freport = new FastReport.Report();

            var resultado = await _reciboInterface.BuscarReciboPorId(id);

            if (resultado == null || resultado.Dados == null)
                return NotFound($"Nenhum orçamento encontrado com numeração {id}");

            freport.Dictionary.RegisterBusinessObject(new List<ReciboModel> {resultado.Dados}, "Recibo", 10, true);
            freport.Report.Load(reportFile);
            freport.Prepare();
            var pdfExport = new PDFSimpleExport();

            using MemoryStream ms = new MemoryStream();
            pdfExport.Export(freport, ms);
            ms.Flush();

            string nomeCliente = resultado.Dados.Cliente.Nome;

            string nomeBase = $"Recibo {resultado.Dados.Numeracao} - {nomeCliente}";
            string nomeArquivoLimpo = string.Join("_", nomeBase.Split(Path.GetInvalidFileNameChars()));
            string nomeArquivoPDF = $"{nomeArquivoLimpo}.pdf";

            return File(ms.ToArray(), "application/pdf", nomeArquivoPDF);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}