using System.Security.Cryptography;
using System.Text;

namespace BienenstockCorpAPI.Helpers
{
    public class EncryptionHelper
    {
        public static string EncryptSHA256(string input)
        {
            SHA256 sha256 = SHA256.Create();

            ASCIIEncoding encoding = new();

            StringBuilder stringBuilder = new();

            var stream = sha256.ComputeHash(encoding.GetBytes(input));

            for (int i = 0; i < stream.Length; i++) stringBuilder.AppendFormat("{0:x2}", stream[i]);

            return stringBuilder.ToString();
        }
    }
}
