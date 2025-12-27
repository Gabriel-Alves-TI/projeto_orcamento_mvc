using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services.ReciboService
{
    public class ReciboService : IReciboInterface
    {
        private readonly AppDbContext _context;
        public ReciboService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<ReciboModel>>> BuscarRecibos()
        {
            var response = new ResponseModel<List<ReciboModel>>();

            try
            {
                var recibos = await _context.Recibos
                    .Include(r => r.Cliente)
                    .ToListAsync();

                if (!recibos.Any())
                {
                    response.Mensagem = "Nenhum recibo localizado.";
                    response.Status = false;
                    return response;
                }

                response.Dados = recibos;
                response.Mensagem = $"Listando {recibos.Count} Recibos localizados.";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ReciboModel>> BuscarReciboPorId(int id)
        {
            var response = new ResponseModel<ReciboModel>();

            try
            {
                var recibo = await _context.Recibos
                    .Include(r => r.Cliente)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (recibo == null)
                {
                    response.Mensagem = "Nenhum orçamento localizado";
                    response.Status = false;
                    return response;
                }

                response.Dados = recibo;
                response.Mensagem = "Orçamentos carregados com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }
        public async Task<ResponseModel<ReciboModel>> CriarRecibo(ReciboDTO reciboDto)
        {
           var response = new ResponseModel<ReciboModel>();

            try
            {
                if (NumeracaoExistente(reciboDto.Numeracao))
                {
                    response.Mensagem = "Já existe um Recibo com essa numeração. Edite/Exclua o Recibo existente ou altere a numeração.";
                    response.Status = false;
                    return response;
                }
                else
                {
                    var cliente = _context.Clientes.FirstOrDefault(c => c.Nome == reciboDto.Cliente);

                    if (cliente == null)
                    {
                        response.Mensagem = "Cliente não encontrado. Cadastre o cliente para prosseguir!";
                        return response;
                    }

                    var recibo = new ReciboModel
                    {
                        Numeracao = reciboDto.Numeracao,
                        Data = reciboDto.Data,
                        Cliente = cliente,
                        Descricao = reciboDto.Descricao,
                        ValorRecibo = reciboDto.ValorRecibo,
                        Vencimento = reciboDto.Vencimento,
                    };

                    _context.Recibos.Add(recibo);
                    _context.SaveChanges();

                    response.Dados = recibo;
                    response.Mensagem = $"Criado Recibo Nº {recibo.Numeracao} Recibos localizados.";
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

        public async Task<ResponseModel<ReciboModel>> EditarRecibo(ReciboDTO reciboDto)
        {
            var response = new ResponseModel<ReciboModel>();

            try
            {
                var recibo = await _context.Recibos
                    .Include(r => r.Cliente)
                    .FirstOrDefaultAsync(r => r.Id == reciboDto.Id);
                
                var numeracaoDto = reciboDto.Numeracao;
                var numeracaoAntes = recibo.Numeracao;

                var cliente = _context.Clientes.FirstOrDefault(c => c.Nome == reciboDto.Cliente);

                if (numeracaoAntes != numeracaoDto)
                {
                    if (NumeracaoExistente(reciboDto.Numeracao))
                    {
                        response.Mensagem = "Já existe um Recibo com essa numeração. Edite/Exclua o Recibo existente ou altere a numeração.";
                        response.Status = false;
                        return response;
                    }
                    else
                    {
                        if (cliente == null)
                        {
                            response.Mensagem = "Cliente não encontrado. Cadastre o cliente para prosseguir!";
                            return response;
                        }

                        if (recibo == null)
                        {
                            response.Mensagem = "Nenhum recibo localizado.";
                            response.Status = false;
                            return response;
                        }

                        recibo.Numeracao = reciboDto.Numeracao;
                        recibo.Data = reciboDto.Data;
                        recibo.Cliente = cliente;
                        recibo.Descricao = reciboDto.Descricao;
                        recibo.ValorRecibo = reciboDto.ValorRecibo;
                        recibo.Vencimento = reciboDto.Vencimento;

                        _context.Recibos.Update(recibo);
                        await _context.SaveChangesAsync();

                        response.Dados = recibo;
                        response.Mensagem = "Recibo atualizado com sucesso!";
                        return response;
                    }                
                }
                else
                {
                    if (cliente == null)
                    {
                        response.Mensagem = "Cliente não encontrado. Cadastre o cliente para prosseguir!";
                        return response;
                    }

                    if (recibo == null)
                    {
                        response.Mensagem = "Nenhum recibo localizado.";
                        response.Status = false;
                        return response;
                    }

                    recibo.Data = reciboDto.Data;
                    recibo.Cliente = cliente;
                    recibo.Descricao = reciboDto.Descricao;
                    recibo.ValorRecibo = reciboDto.ValorRecibo;
                    recibo.Vencimento = reciboDto.Vencimento;

                    _context.Recibos.Update(recibo);
                    await _context.SaveChangesAsync();

                    response.Dados = recibo;
                    response.Mensagem = "Recibo atualizado com sucesso!";
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

        public async Task<ResponseModel<ReciboModel>> ExcluirRecibo(int id)
        {
            var response = new ResponseModel<ReciboModel>();

            try
            {
                var recibo = await _context.Recibos
                    .Include(r => r.Cliente)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (recibo == null)
                {
                    response.Mensagem = "Nenhum recibo localizado.";
                    response.Status = false;
                    return response;
                }

                _context.Recibos.Remove(recibo);
                await _context.SaveChangesAsync();

                response.Mensagem = "Recibo excluído com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        private bool NumeracaoExistente(int numeracao)
        {
            var orcamento = _context.Recibos.FirstOrDefault(r => r.Numeracao == numeracao);

            if (orcamento != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}