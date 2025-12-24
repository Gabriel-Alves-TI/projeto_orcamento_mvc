using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services
{
    public class OrcamentoService : IOrcamentoInterface
    {
        private readonly AppDbContext _context;

        public OrcamentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<OrcamentoModel>>> BuscarOrcamentos()
        {
            var response = new ResponseModel<List<OrcamentoModel>>();

            try
            {
                var orcamentos = await _context.Orcamentos
                    .Include(orc => orc.Cliente)
                    .Include(orc => orc.Itens)
                    .ToListAsync();

                if (!orcamentos.Any())
                {
                    response.Mensagem = "Nenhum orçamento localizado";
                    response.Status = false;
                    return response;
                }

                response.Dados = orcamentos;
                response.Mensagem = $"Listando {orcamentos.Count} Orçamentos localizados";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrcamentoModel>> BuscarOrcamentoPorId(int id)
        {
            var response = new ResponseModel<OrcamentoModel>();

            try
            {
                var orcamento = await _context.Orcamentos
                    .Include(orc => orc.Cliente)
                    .Include(orc => orc.Itens)
                    .FirstOrDefaultAsync(orc => orc.Id == id);

                if (orcamento == null)
                {
                    response.Mensagem = "Nenhum orçamento localizado";
                    response.Status = false;
                    return response;
                }

                response.Dados = orcamento;
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

        public async Task<ResponseModel<OrcamentoModel>> CriarOrcamento(OrcamentoDTO orcamentoDto)
        {
            var response = new ResponseModel<OrcamentoModel>();

            try
            {
                var cliente = _context.Clientes.FirstOrDefault(c => c.Nome == orcamentoDto.Cliente);

                if (cliente == null)
                {
                    response.Mensagem = "Cliente não localizado. Faça o cadastro para prosseguir.";
                    response.Status = false;
                    return response;
                }

                if (NumeracaoExistente(orcamentoDto.Numeracao))
                {
                    response.Mensagem = "Já existe um Orçamento com essa numeração. Edite/Exclua o Orçamento existente ou altere a numeração.";
                    response.Status = false;
                    return response;
                }
                else
                {
                    var orcamento = new OrcamentoModel
                    {
                        Numeracao = orcamentoDto.Numeracao,
                        Data = orcamentoDto.Data,
                        Cliente = cliente,
                        ValidadeProposta = orcamentoDto.ValidadeProposta,
                        Previsao = orcamentoDto.Previsao,
                        Itens = new List<ItemModel>(),
                    };

                    _context.Orcamentos.Add(orcamento);
                    await _context.SaveChangesAsync();

                    response.Dados = orcamento;
                    response.Mensagem = "Orçamento criado com sucesso!";
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

        public async Task<ResponseModel<OrcamentoModel>> EditarOrcamento(OrcamentoDTO orcamentoDto)
        {
            var response = new ResponseModel<OrcamentoModel>();

            try
            {
                var orcamentoId = await _context.Orcamentos.FindAsync(orcamentoDto.Id);

                var numeracaoDto = orcamentoDto.Numeracao;
                var numeracaoAntes = orcamentoId.Numeracao;

                var cliente = _context.Clientes.FirstOrDefault(c => c.Nome == orcamentoDto.Cliente);

                if (numeracaoAntes != numeracaoDto)
                {
                    if (NumeracaoExistente(orcamentoDto.Numeracao))
                    {
                        response.Mensagem = "Já existe um Orçamento com essa numeração. Exclua o Orçamento existente ou altere a numeração.";
                        response.Status = false;
                        return response;
                    }

                    if (orcamentoDto.Itens == null)
                    {
                        response.Mensagem = "Não é possível salvar um Orçamento sem itens.";
                        response.Status = false;
                        return response;
                    }
                    else
                    {
                        var orcamento = await _context.Orcamentos
                            .Include(o => o.Cliente)
                            .Include(o => o.Itens)
                            .FirstOrDefaultAsync(orc => orc.Id == orcamentoDto.Id);

                        if (orcamento == null)
                        {
                            response.Mensagem = "Orçamento não encontrado.";
                            response.Status = false;
                            return response;
                        }

                        orcamento.Numeracao = orcamentoDto.Numeracao;
                        orcamento.Data = orcamentoDto.Data;
                        orcamento.Cliente = cliente;
                        orcamento.ValidadeProposta = orcamentoDto.ValidadeProposta;
                        orcamento.Previsao = orcamentoDto.Previsao;

                        _context.Orcamentos.Update(orcamento);
                        await _context.SaveChangesAsync();

                        response.Mensagem = "Orçamento atualizado com sucesso!";
                        return response;
                    }
                }
                else
                {
                    if (orcamentoDto.Itens == null)
                    {
                        response.Mensagem = "Não é possível salvar um Orçamento sem itens.";
                        response.Status = false;
                        return response;
                    }
                    else
                    {
                        var orcamento = await _context.Orcamentos
                            .Include(o => o.Cliente)
                            .Include(o => o.Itens)
                            .FirstOrDefaultAsync(orc => orc.Id == orcamentoDto.Id);

                        if (orcamento == null)
                        {
                            response.Mensagem = "Orçamento não encontrado.";
                            response.Status = false;
                            return response;
                        }

                        orcamento.Data = orcamentoDto.Data;
                        orcamento.Cliente = cliente;
                        orcamento.ValidadeProposta = orcamentoDto.ValidadeProposta;
                        orcamento.Previsao = orcamentoDto.Previsao;

                        _context.Orcamentos.Update(orcamento);
                        await _context.SaveChangesAsync();

                        response.Mensagem = "Orçamento atualizado com sucesso!";
                        return response;
                    }
                }   
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<OrcamentoModel>> ExcluirOrcamento(int id)
        {
            var response = new ResponseModel<OrcamentoModel>();

            try
            {
                var orcamento = _context.Orcamentos
                    .Include(orc => orc.Cliente)
                    .Include(orc => orc.Itens)
                    .FirstOrDefault(orc => orc.Id == id);

                if (orcamento == null)
                {
                    response.Mensagem = "Orçamento não localizado.";
                    response.Status = false;
                    return response;
                }

                var itensOrcamento = _context.Itens.Where(i => i.OrcamentoId == orcamento.Id).ToList();

                _context.Itens.RemoveRange(itensOrcamento);
                _context.Orcamentos.Remove(orcamento);

                await _context.SaveChangesAsync();

                response.Mensagem = "Orçamento excluído com sucesso.";
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
            var orcamento = _context.Orcamentos.FirstOrDefault(orc => orc.Numeracao == numeracao);

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