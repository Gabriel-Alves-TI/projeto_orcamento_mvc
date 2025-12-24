using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastReport;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services.ReportService
{
    public interface IReportInterface
    {
        Task<ResponseModel<byte[]>> CriarRelatorio(int orcamentoId);
        Task<ResponseModel<byte[]>> CriarRelatorioRecibo(int reciboId);
    }
}