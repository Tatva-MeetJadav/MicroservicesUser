using System.Security.Cryptography;
using MicroservicesUser.BusinessLogic.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MicroservicesUser.BusinessLogic.Implementations
{
    public class EncryptDecryptServices : IEncryptDecryptServices
    {
        private readonly IConfiguration _configuration;
        public EncryptDecryptServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EncryptPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        public string EncryptId(int id)
        {
            byte[] key = Convert.FromBase64String(_configuration["EncryptId:Key"] ?? string.Empty);
            byte[] iv = Convert.FromBase64String(_configuration["EncryptId:IV"] ?? string.Empty);
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream ms = new();
            using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
            using (StreamWriter sw = new(cs))
            {
                sw.Write(id.ToString());
            }
            return Convert.ToBase64String(ms.ToArray())
                .Replace('+', '-').Replace('/', '_').Replace("=", "");
        }

        public int DecryptId(string encrypted)
        {
            byte[] key = Convert.FromBase64String(_configuration["EncryptId:Key"] ?? string.Empty);
            byte[] iv = Convert.FromBase64String(_configuration["EncryptId:IV"] ?? string.Empty);
            string incoming = encrypted.Replace('-', '+').Replace('_', '/');
            switch (incoming.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            byte[] buffer = Convert.FromBase64String(incoming);
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream ms = new(buffer);
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new(cs);
            string decrypted = sr.ReadToEnd();
            return int.Parse(decrypted);
        }
    }
}
