using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services
{
    public interface IOrcamentoInterface
    {
        Task<ResponseModel<List<OrcamentoModel>>> BuscarOrcamentos();
        Task<ResponseModel<OrcamentoModel>> BuscarOrcamentoPorId(int id);
        Task<ResponseModel<OrcamentoModel>> CriarOrcamento(OrcamentoDTO orcamentoDto);
        Task<ResponseModel<OrcamentoModel>> EditarOrcamento(OrcamentoDTO orcamentoDto);
        Task<ResponseModel<OrcamentoModel>> ExcluirOrcamento(int id);
    }
}