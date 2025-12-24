using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.Models;
using projeto_orcamento_mvc.Services;
using projeto_orcamento_mvc.Services.ReciboService;

namespace projeto_orcamento_mvc.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly IOrcamentoInterface _orcamentoInterface;
    private readonly IReciboInterface _reciboInterface;

    public HomeController(AppDbContext context, IOrcamentoInterface orcamentoInterface, IReciboInterface reciboInterface)
    {
        _context = context;
        _orcamentoInterface = orcamentoInterface;
        _reciboInterface = reciboInterface;
    }

    public async Task<IActionResult> Index()
    {
        var orcamentos = await _orcamentoInterface.BuscarOrcamentos();

        var recibos = await _reciboInterface.BuscarRecibos();

        var viewModel = new HomeIndexView
        {
            Orcamentos = orcamentos.Dados,
            Recibos = recibos.Dados,
        };

        TempData["MensagemSucesso"] = $"{orcamentos.Mensagem} - {recibos.Mensagem}";
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
