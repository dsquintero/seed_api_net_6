using Api.Interfaces;
using System.Security.Cryptography;

namespace Api.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string HashPassword(string password)
        {
            return CifrarSHA256(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            //return Equals(HashPassword(password), passwordHash);
            return HashPassword(password).Equals(passwordHash, StringComparison.OrdinalIgnoreCase);
        }

        private string CifrarSHA256(string inputString)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(inputString);
            byte[] hashedBytes = sha256.ComputeHash(inputBytes);
            string hashedString = BitConverter.ToString(hashedBytes).Replace("-", "");
            return hashedString;
        }
    }
}
