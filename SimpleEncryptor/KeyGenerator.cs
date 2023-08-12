using System.Security.Cryptography;
using System.Text;

namespace SimpleEncryptor
{
    public class keyGenerator
    {
        public static byte[] generateEncryptionKey(string passphrase)
        {
            byte[] password = Encoding.UTF8.GetBytes(passphrase);
            byte[] salt = Encoding.UTF8.GetBytes("staticsalt123456staticsalt123456");
            //byte[] salt = RandomNumberGenerator.GetBytes(32);
            int iterations = 1000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
            int outputLength = 32;

            var encryptionKey = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, outputLength);

            return encryptionKey;
        }
    }
}