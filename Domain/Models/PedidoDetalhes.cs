namespace API.Domain.Models
{
    public class PedidoDetalhes
    {
        public int Id { get; set; }
        public Pedido Pedido { get; set; } = new();
        public Produto Produto { get; set; } = new();
        public int Quantidade { get; set; }
        public double Subtotal { get; set; }
    }
}
