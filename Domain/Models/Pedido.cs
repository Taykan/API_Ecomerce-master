namespace API.Domain.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime DataEnvio { get; set; }
        public Cliente Cliente { get; set; } = new();
        public EStatus Status { get; set; }
        public ETipoEnvio TipoEnvio { get; set; }
    }

    public enum EStatus
    {
        Realizado = 1,
        Processamento = 2,
        Finalizado = 3,
    }

    public enum ETipoEnvio
    {
        Correio = 1,
        Transportadora = 2,
        Retirada = 3
    }
}
