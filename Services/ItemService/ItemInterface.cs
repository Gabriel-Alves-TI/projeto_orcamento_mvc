using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projeto_orcamento_mvc.DTO;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Services.ItemService
{
    public interface ItemInterface
    {
        Task<ResponseModel<ItemModel>> AdicionarItem(ItemDTO itemDto);
        Task<ResponseModel<ItemModel>> EditarItem(ItemDTO itemDto);
        Task<ResponseModel<ItemModel>> ExcluirItem(ItemDTO itemDto);
        Task<ResponseModel<List<ItemModel>>> ListarItens(int orcamentoId);
    }
}