namespace SimpleEncryptor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var mode = args[0];
                var inputFile = args[1];
                var outputFile = args[2];

                if (!(File.Exists(inputFile)))
                {
                    Console.WriteLine($"[!] Input file ({inputFile}) does not exist.");
                    Environment.Exit(1);
                }

                if (File.Exists(outputFile))
                {
                    Console.WriteLine($"[!] Output file ({outputFile}) already exists. Use a different path or filename.");
                    Environment.Exit(1);
                }

                switch (mode.ToLower())
                {
                    case "encrypt":
                        Console.WriteLine($"[+] Enter a passphrase to encrypt the file ({inputFile}) with: ");
                        Encryptor.encrypt(inputFile, outputFile, buildPassphrase());
                        break;

                    case "decrypt":
                        Console.WriteLine($"[+] Enter the correct passphrase to decrypt the file ({inputFile}): ");
                        Decryptor.decrypt(inputFile, outputFile, buildPassphrase());
                        break;

                    case "help":
                        displayHelpMessageInConsole();
                        break;
                }
            }
            catch (IndexOutOfRangeException)
            {
                displayHelpMessageInConsole();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void displayHelpMessageInConsole()
        {
            Console.WriteLine(@"[?] SimpleEncryptor encrypt c:\tools\passwords.txt c:\tools\passwords.txt.aes");
            Console.WriteLine(@"[?] SimpleEncryptor decrypt c:\tools\passwords.txt.aes c:\tools\passwords2.txt");
        }

        public static string buildPassphrase()
        {
            string passphrase = string.Empty;

            while (true)
            {
                var key = Console.ReadKey(true);
                char passphraseChar = key.KeyChar;

                if (passphraseChar == (char)ConsoleKey.Enter)
                {
                    break;
                }
                else if (passphraseChar == (char)ConsoleKey.Backspace)
                {
                    try
                    {
                        passphrase = passphrase.Remove(passphrase.Length - 1);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        continue;
                    }
                }
                else
                {
                    passphrase += passphraseChar;
                }
            }
            return passphrase;
        }
    }
}