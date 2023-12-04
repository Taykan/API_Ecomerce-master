using API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Infraestructure.Data
{
    public class DataContent : DbContext
    {
        public DataContent(DbContextOptions<DataContent> options) : base(options)
        {
        }

        public DbSet<ItemCarrinho> ItemCarrinho { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<PedidoDetalhes> PedidoDetalhes { get; set; }
        public DbSet<Produto> Produto { get; set; }
    }
}
