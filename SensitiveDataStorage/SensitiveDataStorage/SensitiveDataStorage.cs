using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SensitiveDataStorage
{
    public class SensitiveDataStorage
    {
        private readonly string applicationFolder = Environment.ExpandEnvironmentVariables(@"%AppData%\" + Process.GetCurrentProcess().ProcessName);

        public string EncryptionPassword = "";

        public void Create(string fileName)
        {
            string filePath = GetPathFromFileName(fileName);

            Directory.CreateDirectory(applicationFolder);

            if (!File.Exists(filePath))
                using (FileStream fs = File.Create(filePath));
        }

        /// <param name="ID">Behaves like an Array. (Starts on 0)</param>
        public string ReadLine(string fileName, int ID)
        {
            string filePath = GetPathFromFileName(fileName);

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Can't read from non-existing file: {0}", filePath);
                return "ERROR";
            }
            if (EncryptionPassword == "")
            {
                Console.WriteLine("Can't read line.\nSensitiveDataStorage.EncryptionPassword is not assigned.");
                return null;
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Count() < ID + 1 || lines[ID].Length < 1) return "";

            try
            {
                return Encrypt.DecryptString(lines[ID], EncryptionPassword);
            }
            catch
            {
                Console.WriteLine("Couln't decrypt Line:{1} in File:{0}", filePath, ID);
                return "ERROR";
            }
        }

        /// <param name="ID">Behaves like an Array. (Starts on 0)</param>
        public void WriteLine(string fileName, int ID, string input)
        {
            string filePath = GetPathFromFileName(fileName);

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Can't write to non-existing file: {0}", filePath);
                return;
            }
            if (EncryptionPassword == "")
            {
                Console.WriteLine("Can't write line.\nSensitiveDataStorage.EncryptionPassword is not assigned.");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            Array.Resize(ref lines, ID + 1);
            lines[ID] = Encrypt.EncryptString(input, EncryptionPassword);
            File.WriteAllLines(GetPathFromFileName(fileName), lines);
        }

        private string GetPathFromFileName(string fileName)
        {
            return applicationFolder + @"\" + fileName + ".txt";
        }
    }

    static class Encrypt
    {
        private const string initVector = "pemgail9uzpgzl88";
        private const int keysize = 256;

        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}
