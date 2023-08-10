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
        public static void decrypt(string fileLocation, string decryptedOutFileLocation, string passphrase)
        {
            var keyGenerator = new keyGenerator();
            var key = keyGenerator.generateEncryptionKey(passphrase);

            FileStream fileStreamInputFile = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);
            FileStream fileStreamOutputFile = new FileStream(decryptedOutFileLocation, FileMode.Create);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.IV = Encoding.UTF8.GetBytes("1234567812345678");  // first block only

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                CryptoStream cryptoStream = new CryptoStream(fileStreamInputFile, decryptor, CryptoStreamMode.Read);

                try
                {
                    byte[] buffer = new byte[1024];
                    int read;

                    while ((read = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fileStreamOutputFile.Write(buffer, 0, read);
                        fileStreamOutputFile.Flush();
                    }
                    Console.WriteLine($"[+] Decryption Successful ({decryptedOutFileLocation}).");
                }
                catch (CryptographicException)
                {
                    Console.WriteLine("[!] Incorrect Passphrase");
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