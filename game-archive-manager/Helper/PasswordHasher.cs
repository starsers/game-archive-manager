using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace game_archive_manager.Helper
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            string hashedInput = HashPassword(inputPassword);
            return hashedInput.Equals(hashedPassword);
        }
        public static PasswordStrength CheckPasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return PasswordStrength.Weak;

            bool hasDigit = password.Any(char.IsDigit);
            bool hasLetter = password.Any(char.IsLetter);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (hasDigit && hasLetter && hasSpecial)
                return PasswordStrength.Strong;
            else if (hasDigit && hasLetter)
                return PasswordStrength.Medium;
            else
                return PasswordStrength.Weak;
        }

        public enum PasswordStrength
        {
            Weak,
            Medium,
            Strong
        }
    }
}
