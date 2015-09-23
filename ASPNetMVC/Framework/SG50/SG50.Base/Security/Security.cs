using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Base.Security
{
    public static class Security
    {
        #region Hashing - SHA 2
        public static byte[] GetSaltKey(int length)
        {
            byte[] data = new byte[length];
            using (RNGCryptoServiceProvider _RNGCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                _RNGCryptoServiceProvider.GetBytes(data);
            }
            return data;
        }

        /// <summary>
        /// SHA 2 (512) 
        /// Salt
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string HashCode(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA512Managed();
            byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }

            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes));
        }
        #endregion

        #region Cryptography - AES 256
        public static string Encrypt(byte[] SymmetricKey, byte[] Vector, string PlainText)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.BlockSize = 128;
            aes.KeySize = 256;

            // It is equal in java 
            /// Cipher _Cipher = Cipher.getInstance("AES/CBC/PKCS5PADDING");    
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] KeyArrBytes32Value = new byte[32];
            Array.Copy(SymmetricKey, KeyArrBytes32Value, 32);

            byte[] IVBytes16Value = new byte[16];
            Array.Copy(Vector, IVBytes16Value, 16);

            aes.Key = KeyArrBytes32Value;
            aes.IV = IVBytes16Value;

            ICryptoTransform encrypto = aes.CreateEncryptor();

            byte[] plainTextByte = ASCIIEncoding.UTF8.GetBytes(PlainText);
            byte[] CipherText = encrypto.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);
            return Convert.ToBase64String(CipherText);
        }

        public static string Decrypt(byte[] SymmetricKey, byte[] Vector, string CipherText)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.BlockSize = 128;
            aes.KeySize = 256;

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] KeyArrBytes32Value = new byte[32];
            Array.Copy(SymmetricKey, KeyArrBytes32Value, 32);

            byte[] IVBytes16Value = new byte[16];
            Array.Copy(Vector, IVBytes16Value, 16);

            aes.Key = KeyArrBytes32Value;
            aes.IV = IVBytes16Value;

            ICryptoTransform decrypto = aes.CreateDecryptor();

            byte[] encryptedBytes = Convert.FromBase64CharArray(CipherText.ToCharArray(), 0, CipherText.Length);
            byte[] decryptedData = decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return ASCIIEncoding.UTF8.GetString(decryptedData);
        }
        #endregion
    }
}
