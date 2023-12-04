namespace API.Domain.Models
{
    public class ItemCarrinho
    {
        public int Id { get; set; }
        public Produto Produto { get; set; } = new Produto();
        public int Quantidade { get; set; }
        public DateTime DataAdicao { get; set; }
    }
}
