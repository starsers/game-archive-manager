using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_archive_manager.Helper
{
    internal static class FileHelper
    {
        public static string GetFileType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            var extension = Path.GetExtension(fileName);
            return string.IsNullOrEmpty(extension) ? "unknown" : extension.ToLower();
        }
        // 打开AES加密文件
        public static string OpenEncryptedFile(string filePath, string password)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("File path and password cannot be null or empty.");
            }
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }
            try
            {
                // Read the encrypted file content
                byte[] encryptedContent = File.ReadAllBytes(filePath);

                // Decrypt the content using the provided password（使用c#的AES解密）
                using (var aes = System.Security.Cryptography.Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(password.PadRight(32).Substring(0, 32)); // Ensure key is 32 bytes
                    aes.IV = new byte[16]; // Use a zero IV for simplicity, but this is not secure
                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var msDecrypt = new MemoryStream(encryptedContent))
                    using (var csDecrypt = new System.Security.Cryptography.CryptoStream(msDecrypt, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        using (var msResult = new MemoryStream())
                        {
                            csDecrypt.CopyTo(msResult);
                            byte[] decryptedContent = msResult.ToArray();
                            return Encoding.UTF8.GetString(decryptedContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to open or decrypt the file.", ex);
            }
        }
        // Save AES加密文件
        public static void SaveEncryptedFile(string filePath, string content, string password)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("File path, content, and password cannot be null or empty.");
            }
            try
            {
                // Encrypt the content using the provided password
                using (var aes = System.Security.Cryptography.Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(password.PadRight(32).Substring(0, 32)); // Ensure key is 32 bytes
                    aes.IV = new byte[16]; // Use a zero IV for simplicity, but this is not secure
                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (var msEncrypt = new MemoryStream())
                    using (var csEncrypt = new System.Security.Cryptography.CryptoStream(msEncrypt, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
                        csEncrypt.Write(contentBytes, 0, contentBytes.Length);
                        csEncrypt.FlushFinalBlock();
                        File.WriteAllBytes(filePath, msEncrypt.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save or encrypt the file.", ex);
            }
        }
    }
}
