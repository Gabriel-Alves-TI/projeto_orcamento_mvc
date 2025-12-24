using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services.ClienteService
{
    public class ClienteService : IClienteInterface
    {
        private readonly AppDbContext _context;
        public ClienteService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<List<ClienteModel>>> BuscarClientes()
        {
            var response = new ResponseModel<List<ClienteModel>>();

            try
            {
                var clientes = await _context.Clientes
                    .OrderBy(c => c.Nome)
                    .ToListAsync();

                if (!clientes.Any())
                {
                    response.Mensagem = "Nenhum cliente localizado.";
                    response.Status = false;
                    return response;
                }

                response.Dados = clientes;
                response.Mensagem = $"Listando {clientes.Count} Clientes localizados.";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ClienteModel>> CadastrarCliente(ClienteDTO clienteDto)
        {
            var response = new ResponseModel<ClienteModel>();

            try
            {   
                var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nome == clienteDto.Nome);

                if (clienteExistente != null && clienteExistente.CpfCnpj == clienteDto.CpfCnpj)
                {
                    response.Mensagem = "Já existe um cliente cadastrado com esses dados!";
                    response.Status = false;
                    return response;
                }
                else
                {
                    var cliente = new ClienteModel
                    {
                        Nome = clienteDto.Nome,
                        CpfCnpj = clienteDto.CpfCnpj,
                        Cep = clienteDto.Cep,
                        Logradouro = clienteDto.Logradouro,
                        Numero = clienteDto.Numero,
                        Cidade = clienteDto.Cidade,
                        Estado = clienteDto.Estado,
                        Telefone = clienteDto.Telefone,
                    };

                    _context.Clientes.Add(cliente);
                    await _context.SaveChangesAsync();

                    response.Dados = cliente;
                    response.Mensagem = "Cliente cadastrado com sucesso!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ClienteModel>> EditarCliente(ClienteDTO clienteDto)
        {
            var response = new ResponseModel<ClienteModel>();

            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == clienteDto.Id);

                if (cliente == null)
                {
                    response.Mensagem = "Cliente não localizado.";
                    response.Status = false;
                    return response;
                }
                
                var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nome == clienteDto.Nome  && c.CpfCnpj == clienteDto.CpfCnpj && c.Id != clienteDto.Id);

                if (clienteExistente != null)
                {
                    response.Mensagem = "Já existe um cliente cadastrado com esses dados. Edite/Exclua o outro cadastro para prosseguir.";
                    response.Status = false;
                    return response;
                }

                cliente.Nome = clienteDto.Nome;
                cliente.CpfCnpj = clienteDto.CpfCnpj;
                cliente.Cep = clienteDto.Cep;
                cliente.Logradouro = clienteDto.Logradouro;
                cliente.Numero = clienteDto.Numero;
                cliente.Cidade = clienteDto.Cidade;
                cliente.Estado = clienteDto.Estado;
                cliente.Telefone = clienteDto.Telefone;

                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();

                response.Dados = cliente;
                response.Mensagem = "Cliente atualizado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ClienteModel>> ExcluirCliente(int id)
        {
            var response = new ResponseModel<ClienteModel>();

            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    response.Mensagem = "Nenhum cliente localizado.";
                    response.Status = false;
                    return response;
                }

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                response.Mensagem = "Cliente excluído com sucesso!";
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