using API.Cryptography;

namespace API.Domain.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string? Endereco { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

        public bool SenhaValida(string senha)
        {
            return Senha == senha.GerarHash();
        }

        public void SenhaSetHash()
        {
            Senha = Senha.GerarHash();
        }
    }
}
