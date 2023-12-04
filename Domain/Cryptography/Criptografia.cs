using System.Security.Cryptography;
using System.Text;

namespace API.Cryptography
{
    public static class Criptografia
    {
        public static string GerarHash(this string valor)
        {
            var hash = SHA1.Create();
            var encoding = new ASCIIEncoding();
            var array = encoding.GetBytes(valor);

            array = hash.ComputeHash(array);

            var strHexa = new StringBuilder();

            foreach (var b in array)
            {
                strHexa.Append(b.ToString("x2"));
            }

            return strHexa.ToString();
        }
    }
}
