using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEncryptor
{
    internal class Decryptor
    {
        public static void decrypt(string cipherFileLocation, string decryptedOutFileLocation, string passphrase)
        {
            var keyGenerator = new keyGenerator();
            var key = keyGenerator.generateEncryptionKey(passphrase);

            FileStream fp = new FileStream(cipherFileLocation, FileMode.Open, FileAccess.Read);
            FileStream decryptedFileStream = new FileStream(decryptedOutFileLocation, FileMode.Create);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.IV = Encoding.UTF8.GetBytes("1234567812345678");  // first block only

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                CryptoStream cryptoStream = new CryptoStream(fp, decryptor, CryptoStreamMode.Read);

                try
                {
                    byte[] buffer = new byte[1024];
                    int read;

                    while ((read = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        decryptedFileStream.Write(buffer, 0, read);
                        decryptedFileStream.Flush();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    fp.Close();
                    decryptedFileStream.Close();
                }
            }
        }
    }
}