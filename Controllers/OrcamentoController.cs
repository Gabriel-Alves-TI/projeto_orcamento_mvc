using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Models;
using projeto_orcamento_mvc.Services;
using projeto_orcamento_mvc.Services.ReportService;

namespace projeto_orcamento_mvc.Controllers
{
    public class OrcamentoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IOrcamentoInterface _orcamentoInterface;
        private readonly IReportInterface _reportInterface;
        private readonly IWebHostEnvironment _webHostEnv;

        public OrcamentoController(AppDbContext context, IOrcamentoInterface orcamentoInterface, IReportInterface reportInterface, IWebHostEnvironment webHostEnv)
        {
            _context = context;
            _orcamentoInterface = orcamentoInterface;
            _reportInterface = reportInterface;
            _webHostEnv = webHostEnv;
        }

        [HttpGet("Orcamento/SomaTotalMaterial")]
        public IActionResult SomaTotalMaterial(int orcamentoId)
        {
            var somaMaterial = _context.Itens
                .Where(i => i.OrcamentoId == orcamentoId)
                .Sum(i => i.ValorMaterial);

            return Json(somaMaterial);
        }

        public IActionResult NovoOrcamento()
        {
            int proximaNumeracao = 1;

            var ultimoOrcamento = _context.Orcamentos
                .OrderByDescending(orc => orc.Numeracao)
                .FirstOrDefault();

            if (ultimoOrcamento != null)
            {
                proximaNumeracao = ultimoOrcamento.Numeracao + 1;
            }

            var novoOrcamento = new OrcamentoDTO
            {
                Numeracao = proximaNumeracao
            };

            ViewBag.datenow = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
            return View(novoOrcamento);
        }

        [HttpPost]
        public async Task<IActionResult> NovoOrcamento(OrcamentoDTO orcamentoDto)
        {
            if (ModelState.IsValid)
            {
                var novoOrcamento = await _orcamentoInterface.CriarOrcamento(orcamentoDto);

                if (novoOrcamento.Status)
                {
                    TempData["MensagemSucesso"] = novoOrcamento.Mensagem;
                    
                }
                else
                {
                    TempData["MensagemErro"] = novoOrcamento.Mensagem;
                    ViewBag.datenow = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                    return View(orcamentoDto);
                }

                return RedirectToAction("Cadastrar", new { id = novoOrcamento.Dados.Id});
            }
            else
            {
                TempData["MensagemErro"] = "Ocorreu algum erro ao salvar o novo Orçamento";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult Cadastrar(int id)
        {
            var orcamento = _context.Orcamentos
                .Include(o => o.Cliente)
                .FirstOrDefault(o => o.Id == id);

            if (orcamento == null)
            {
                TempData["MensagemErro"] = "Orçamento não localizado.";
                return RedirectToAction("Index", "Home");
            }

            var novoOrcamento = new OrcamentoDTO
            {
                Id = orcamento.Id,
                Numeracao = orcamento.Numeracao,
                Data = orcamento.Data,
                Cliente = orcamento.Cliente.Nome,
                ClienteId = orcamento.ClienteId,

                //Dados Cliente
                CpfCnpj = orcamento.Cliente.CpfCnpj,
                Cep = orcamento.Cliente.Cep.ToString(),
                Logradouro = orcamento.Cliente.Logradouro,
                Numero = orcamento.Cliente.Numero.ToString(),
                Cidade = orcamento.Cliente.Cidade,
                Estado = orcamento.Cliente.Estado,
                Telefone = orcamento.Cliente.Telefone,

                ValidadeProposta = orcamento.ValidadeProposta,
                Previsao = orcamento.Previsao,
                Itens = [],
            };

            ViewBag.somaMaterial = orcamento.Itens.Sum(i => i.ValorMaterial);
            ViewBag.datenow = DateTime.Now;
            return View(novoOrcamento);
        }

        // [HttpPost]
        // public async Task<IActionResult> Cadastrar(OrcamentoDTO orcamentoDto)
        // {

        //     if (!ModelState.IsValid)
        //     {
        //         Console.Write("Está com erro na Model aqui...");
        //         foreach (var entry in ModelState)
        //         {
        //             foreach (var error in entry.Value.Errors)
        //             {
        //                 Console.WriteLine($"Erro no campo {entry.Key}: {error.ErrorMessage}");
        //             }
        //         }

        //         return View(orcamentoDto);
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         Console.Write("Entrou aqui...");

        //         var orcamento = await _orcamentoInterface.CriarOrcamento(orcamentoDto);

        //         if (orcamento.Status)
        //         {
        //             TempData["MensagemSucesso"] = orcamento.Mensagem;
        //             Console.WriteLine($"orcamento.Dados = {orcamento.Dados}");
        //             Console.WriteLine($"orcamento.Dados.Numeracao = {orcamento.Dados?.Numeracao}");
        //         }
        //         else
        //         {
        //             TempData["MensagemErro"] = orcamento.Mensagem;
        //             return RedirectToAction("Index", "Home");
        //         }

        //         // return RedirectToAction("OrcamentoRelatorio", new { numeracao = orcamento.Dados.Numeracao });
        //         return View(orcamentoDto);
        //     }

        //     TempData["MensagemErro"] = "Ocorreu algum erro ao salvar o Orçamento.";
        //     return View(orcamentoDto);
        // }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var orcamento = _context.Orcamentos
                .Include(orc => orc.Cliente)
                .Include(orc => orc.Itens)
                .FirstOrDefault(orc => orc.Id == id);

            if (orcamento == null)
            {
                return NotFound();
            }

            var orcamentoView = new OrcamentoDTO
            {
                Id = orcamento.Id,
                Numeracao = orcamento.Numeracao,
                Data = orcamento.Data,
                Cliente = orcamento.Cliente.Nome,
                ClienteId = orcamento.ClienteId,

                //Dados Cliente
                CpfCnpj = orcamento.Cliente.CpfCnpj,
                Cep = orcamento.Cliente.Cep.ToString("00000-000"),
                Logradouro = orcamento.Cliente.Logradouro,
                Numero = orcamento.Cliente.Numero.ToString(),
                Cidade = orcamento.Cliente.Cidade,
                Estado = orcamento.Cliente.Estado,
                Telefone = orcamento.Cliente.Telefone,

                ValidadeProposta = orcamento.ValidadeProposta,
                Previsao = orcamento.Previsao,
                Itens = orcamento.Itens,
            };
            
            ViewBag.somaMaterial = orcamento.Itens.Sum(i => i.ValorMaterial);
            return View(orcamentoView);
        }

        [HttpGet]
        public IActionResult EditarItem(int id)
        {
            var item = _context.Itens.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();

            return PartialView("_EditarItem", item);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(OrcamentoDTO orcamentoDto)
        {
            if (ModelState.IsValid)
            {
                var orcamento = await _orcamentoInterface.EditarOrcamento(orcamentoDto);

                if (orcamento.Status)
                {
                    TempData["MensagemSucesso"] = orcamento.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = orcamento.Mensagem;
                    return View(orcamentoDto);
                }

                return RedirectToAction("Index", "Home");
            }

            TempData["MensagemErro"] = "Ocorreu algum erro ao atualizar o Orçamento";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            OrcamentoModel orcamento = _context.Orcamentos
                .Include(orc => orc.Cliente)
                .FirstOrDefault(orc => orc.Id == id);

            if (orcamento == null)
            {
                return NotFound();
            }

            return View(orcamento);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var orcamento = await _orcamentoInterface.ExcluirOrcamento(id);

            if (orcamento.Status)
            {
                TempData["MensagemSucesso"] = orcamento.Mensagem;
                return RedirectToAction("Index", "Home");
            }

            TempData["MensagemErro"] = orcamento.Mensagem;
            return RedirectToAction("Index", "Home");
        }

        // MÉTODO PARA GERAR ARQUIVO .FRX

        // public async Task<IActionResult> CreateReport(int id)
        // {
        //     var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"reports/ReportMvc - NovoTemplate.frx");
        //     var reportFile = caminhoReport;

        //     var freport = new FastReport.Report();

        //     var resultado = await _orcamentoInterface.BuscarOrcamentoPorNumeracao(id);

        //     freport.Dictionary.RegisterBusinessObject(new List<OrcamentoModel> {resultado.Dados}, "Orcamento", 10, true);
        //     freport.Report.Save(reportFile);

        //     return Ok($"Relatório gerado: {caminhoReport}");
        // }

        public async Task<IActionResult> OrcamentoRelatorio(int id)
        {
            var caminhoReport = Path.Combine(_webHostEnv.WebRootPath, @"reports/Orcamento.frx");
            var reportFile = caminhoReport;

            var freport = new FastReport.Report();

            var resultado = await _orcamentoInterface.BuscarOrcamentoPorId(id);

            if (resultado == null || resultado.Dados == null)
                return NotFound($"Nenhum orçamento encontrado com numeração {id}");

            freport.Dictionary.RegisterBusinessObject(new List<OrcamentoModel> {resultado.Dados}, "Orcamento", 10, true);
            freport.Report.Load(reportFile);
            freport.Prepare();
            var pdfExport = new PDFSimpleExport();

            using MemoryStream ms = new MemoryStream();
            pdfExport.Export(freport, ms);
            ms.Flush();

            string nomeCliente = resultado.Dados.Cliente.Nome;

            string nomeBase = $"Orçamento {resultado.Dados.Numeracao} - {nomeCliente}";
            string nomeArquivoLimpo = string.Join("_", nomeBase.Split(Path.GetInvalidFileNameChars()));
            string nomeArquivoPDF = $"{nomeArquivoLimpo}.pdf";

            return File(ms.ToArray(), "application/pdf", nomeArquivoPDF);
        }

        // [HttpGet]
        // public async Task<IActionResult> Gerar(int id)
        // {
        //     var resultado = await _reportInterface.CriarRelatorio(id);

        //     if (!resultado.Status)
        //     {
        //         return BadRequest(resultado.Mensagem);
        //     }

        //     return File(resultado.Dados, "application/pdf", $"Orçamento_{id}.pdf");
        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}