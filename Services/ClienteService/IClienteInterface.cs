using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services.ClienteService
{
    public interface IClienteInterface
    {
        Task<ResponseModel<List<ClienteModel>>> BuscarClientes();
        Task<ResponseModel<ClienteModel>> CadastrarCliente(ClienteDTO clienteDto);
        Task<ResponseModel<ClienteModel>> EditarCliente(ClienteDTO clienteDto);
        Task<ResponseModel<ClienteModel>> ExcluirCliente(int id);
    }
}