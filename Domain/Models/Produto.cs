namespace API.Domain.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public string? DescricaoProduto { get; set; }
        public string? Imagem { get; set; }
        public double Preco { get; set; }
    }
}
