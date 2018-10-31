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
        public static string CreateSalt(int size)
        {
            char[] characters = new char[] { 'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b',
            'c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z' };

            char[] buff = new char[size];

            Random rnd = new Random();

            for (int i = 0; i < size; i++)
            {
                buff[i] = characters[rnd.Next(characters.Length)];
            }
            string salt = new string(buff);

            return salt;
        }

        public static byte[] GenerateSHA256Hash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sha256hash = new SHA256Managed();
            byte[] hash = sha256hash.ComputeHash(bytes);

            return hash;
        }

        public static bool CompareByteArray(byte[] arr1, byte[] arr2)
        {
            bool same = true;

            if (arr1.Length != arr2.Length)
            {
                same = false;
            }
            else
            {
                for (int i = 0; i < arr1.Length; i++)
                {
                    if (arr1[i] != arr2[i])
                    {
                        same = false;
                    }
                }
            }

            return same;
        }
    }
}
