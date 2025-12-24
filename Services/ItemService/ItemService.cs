using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projeto_orcamento_mvc.Data;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services.ItemService
{
    public class ItemService : ItemInterface
    {
        private readonly AppDbContext _context;
        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<ItemModel>> AdicionarItem(ItemDTO itemDto)
        {
            var response = new ResponseModel<ItemModel>();

            try
            {   
                if (itemDto == null)
                {
                    response.Mensagem = "Não é possível inserir um item sem informações.";
                    response.Status = false;
                    return response;
                }

                var item = new ItemModel
                {
                    Descricao = itemDto.Descricao,
                    ValorMaterial = itemDto.ValorMaterial,
                    ValorServico = itemDto.ValorServico,
                    OrcamentoId = itemDto.OrcamentoId
                };

                _context.Itens.Add(item);
                await _context.SaveChangesAsync();

                response.Dados = item;
                response.Mensagem = "Item Adicionado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ItemModel>> EditarItem(ItemDTO itemDto)
        {
            var response = new ResponseModel<ItemModel>();

            try
            {
                var item = await _context.Itens.FirstOrDefaultAsync(i => i.Id == itemDto.Id);

                if (item == null)
                {
                    response.Mensagem = "Nenhum item localizado.";
                    response.Status = false;
                    return response;
                }

                item.Descricao = itemDto.Descricao;
                item.ValorMaterial = itemDto.ValorMaterial;
                item.ValorServico = itemDto.ValorServico;

                _context.Itens.Update(item);
                await _context.SaveChangesAsync();


                response.Dados = item;
                response.Mensagem = "Item atualizado com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<ItemModel>> ExcluirItem(ItemDTO itemDto)
        {
            var response = new ResponseModel<ItemModel>();

            try
            {
                var item = await _context.Itens.FirstOrDefaultAsync(i => i.Id == itemDto.Id);

                if (item == null)
                {
                    response.Mensagem = "Nenhum item localizado.";
                    response.Status = false;
                    return response;
                }

                _context.Itens.Remove(item);
                await _context.SaveChangesAsync();

                response.Mensagem = "Item excluído com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<ItemModel>>> ListarItens(int orcamentoId)
        {
            var response = new ResponseModel<List<ItemModel>>();

            try
            {
                var listaItens = await _context.Itens
                    .Where(i => i.OrcamentoId == orcamentoId)
                    .ToListAsync();

                if (!listaItens.Any())
                {
                    response.Mensagem = "Nenhum item localizado.";
                    response.Status = false;
                    return response;
                }

                response.Dados = listaItens;
                response.Mensagem = $"Listando itens {listaItens.Count} localizados";
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