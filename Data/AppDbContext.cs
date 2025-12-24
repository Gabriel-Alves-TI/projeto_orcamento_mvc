using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projeto_orcamento_mvc.Models;

namespace projeto_orcamento_mvc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<OrcamentoModel> Orcamentos { get; set; }
        public DbSet<ItemModel> Itens { get; set; }
        public DbSet<ReciboModel> Recibos { get; set; }
    }
}