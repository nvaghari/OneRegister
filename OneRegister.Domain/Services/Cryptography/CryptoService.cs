using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OneRegister.Domain.Services.Cryptography
{
    public class CryptoService
    {
        public static string SHA256ToHex(string text)
        {
            using var sh = SHA256.Create();
            var textArray = Encoding.UTF8.GetBytes(text);
            var hash = sh.ComputeHash(textArray);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
