
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.DataProtection;

namespace MyBlog.BL.Auth
{
    public class Pbkdf2Encrypt : IEncrypt
    {
        private readonly IDataProtector dataProtector;
        private const string EncryptionKey = "Здесь мы указываем ключ";

        public Pbkdf2Encrypt(IDataProtectionProvider dataProtectionProvider)
        {
            this.dataProtector = dataProtectionProvider.CreateProtector(EncryptionKey);
        }

        public string HashPassword(string password, string salt)
        {
            byte[] saltBytes = System.Text.Encoding.ASCII.GetBytes(salt);

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8));
        }

        public string Encrypt(string text)
        {
            return dataProtector.Protect(text);
        }

        public string Decrypt(string text)
        {
            return dataProtector.Unprotect(text);
        }
    }
}

