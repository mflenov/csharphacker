using System;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace MyBlog.BL.Auth
{
    public class Md5Encrypt: IEncrypt
    {
        public string HashPassword(string password, string salt)
        {
            MD5 md5hash = MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(salt + password);
            byte[] hashBytes = md5hash.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }

        public string Encrypt(string text)
        {
            // этот класс только чтобы показать MD5,
            // здесь этот метод не будет реализовываться
            return text;
        }

        public string Decrypt(string text)
        {
            // этот класс только чтобы показать MD5,
            // здесь этот метод не будет реализовываться
            return text;
        }
    }
}

