using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KH_Capstone_BLL
{
    public class Hashing
    {
        public String CreateSalt(int size)
        {
            RandomNumberGenerator rng = RNGCryptoServiceProvider.Create();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        public byte[] GenerateSHA256Hash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sha256hash = new SHA256Managed();
            byte[] hash = sha256hash.ComputeHash(bytes);

            return bytes;
        }
    }
}
