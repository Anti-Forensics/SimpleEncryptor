using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEncryptor
{
    public class Encryptor
    {
        public static void encrypt(string fileLocation, string encryptedOutFileLocation, string passphrase)
        {
            var keyGenerator = new keyGenerator();
            var key = keyGenerator.generateEncryptionKey(passphrase);

            FileStream fileStreamInputFile = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);
            FileStream fileStreamOutputFile = new FileStream(encryptedOutFileLocation, FileMode.Create);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.IV = Encoding.UTF8.GetBytes("1234567812345678");  // first block only

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                CryptoStream cryptoStream = new CryptoStream(fileStreamOutputFile, encryptor, CryptoStreamMode.Write);

                byte[] buffer = new byte[1024];
                int read;

                try
                {
                    while ((read = fileStreamInputFile.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cryptoStream.Write(buffer, 0, read);
                    }
                    cryptoStream.FlushFinalBlock();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    fileStreamInputFile.Close();
                    fileStreamOutputFile.Close();
                }
            }
        }
    }
}