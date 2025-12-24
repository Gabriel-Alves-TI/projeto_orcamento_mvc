using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services.ReciboService
{
    public interface IReciboInterface
    {
        Task<ResponseModel<List<ReciboModel>>> BuscarRecibos();
        Task<ResponseModel<ReciboModel>> BuscarReciboPorId(int id);
        Task<ResponseModel<ReciboModel>> CriarRecibo(ReciboDTO reciboDto);
        Task<ResponseModel<ReciboModel>> EditarRecibo(ReciboDTO reciboDto);
        Task<ResponseModel<ReciboModel>> ExcluirRecibo(int id);
    }
}