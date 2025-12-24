using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using FastReport;
using FastReport.Export.PdfSimple;
using Microsoft.EntityFrameworkCore;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services.ReportService
{
    public class ReportService : IReportInterface
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnv;

        public ReportService(AppDbContext context, IWebHostEnvironment webHostEnv)
        {
            _context = context;
            _webHostEnv = webHostEnv;
        }

        public async Task<ResponseModel<byte[]>> CriarRelatorio(int orcamentoId)
        {
            var response = new ResponseModel<byte[]>();

            try
            {
                var orcamento = await _context.Orcamentos
                    .Include(orc => orc.Itens)
                    .Include(orc => orc.Cliente)
                    .FirstOrDefaultAsync(orc => orc.Id == orcamentoId);

                if (orcamento == null)
                {
                    response.Mensagem = "Orçamento não encontrado.";
                    response.Status = false;
                    return response;
                }

                var basePath = Directory.GetCurrentDirectory();
                var reportPath = Path.Combine(_webHostEnv.WebRootPath, "reports", "Orcamento.frx");

                if (!System.IO.File.Exists(reportPath))
                    throw new Exception("Arquivo FRX não encontrado: " + reportPath);

                using var report = new Report();
                report.Load(reportPath);

                report.RegisterData(new List<OrcamentoModel> { orcamento }, "Orcamento");
                report.GetDataSource("Orcamento").Enabled = true;

                using var ms = new MemoryStream();
                var pdf = new PDFSimpleExport();

                report.Prepare();
                report.Export(pdf, ms);

                response.Dados = ms.ToArray();
                response.Status = true;
                response.Mensagem = "PDF gerado com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<byte[]>> CriarRelatorioRecibo(int reciboId)
        {
            var response = new ResponseModel<byte[]>();

            try
            {
                var recibo = await _context.Recibos
                    .Include(orc => orc.Cliente)
                    .FirstOrDefaultAsync(orc => orc.Id == reciboId);

                if (recibo == null)
                {
                    response.Mensagem = "Recibo não encontrado.";
                    response.Status = false;
                    return response;
                }

                var basePath = Directory.GetCurrentDirectory();
                var reportPath = Path.Combine(_webHostEnv.WebRootPath, "reports", "Recibo.frx");

                if (!System.IO.File.Exists(reportPath))
                    throw new Exception("Arquivo FRX não encontrado: " + reportPath);

                using var report = new Report();
                report.Load(reportPath);

                report.RegisterData(new List<ReciboModel> { recibo }, "Recibo");
                report.GetDataSource("Recibo").Enabled = true;

                using var ms = new MemoryStream();
                var pdf = new PDFSimpleExport();

                report.Prepare();
                report.Export(pdf, ms);

                response.Dados = ms.ToArray();
                response.Status = true;
                response.Mensagem = "PDF gerado com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }
    }
}