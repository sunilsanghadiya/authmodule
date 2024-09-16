using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace authmodule.Common
{
    internal static class Crypto
    {
        private const int PBKDF2SubKeyLength = 256 / 8;
        private const int SaltSize = 128 / 8;

        /*
         ================
         HASHED PASSWORD
         ================
        */

        public static string HashPassword(string password, int iterationCount)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            byte[] salt;
            byte[] subkey;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, iterationCount))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(PBKDF2SubKeyLength);
            }
            var outputBytes = new byte[1 + SaltSize + PBKDF2SubKeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1, SaltSize);
            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password, int iterationCount)
        {
             if(hashedPassword == null) 
             {
                return false;
             }

             if(password == null) 
             {
                throw new ArgumentNullException(nameof(password));
             }
             
             var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
             
             if(hashedPasswordBytes.Length != (1 + SaltSize + PBKDF2SubKeyLength) || hashedPassword[0] != 0x00)
             {
                return false;
             }

             var salt = new byte[SaltSize];
             Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
             var storedSubKey = new byte[PBKDF2SubKeyLength];
             Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubKey, 0, PBKDF2SubKeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterationCount))
            {
                generatedSubkey = deriveBytes.GetBytes(PBKDF2SubKeyLength);
            }
            return ByteArrayEqual(storedSubKey, generatedSubkey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ByteArrayEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}