using System.Security.Cryptography;
using Application.Interface;
namespace Infrastructure.Service
{
    public class PasswordService : IPasswordService
    {
        private static readonly int SaltSize = 16; // Example salt size in bytes
        private static readonly int HashSize = 32; // Example hash size in bytes
        private static readonly int Iterations = 10000; // Example number of iterations for hashing

        public string HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithmName.SHA1, HashSize);
            byte[] hashWithSaltBytes = new byte[SaltSize + HashSize];
            Array.Copy(saltBytes, 0, hashWithSaltBytes, 0, SaltSize);
            Array.Copy(hashBytes, 0, hashWithSaltBytes, SaltSize, HashSize);
            return Convert.ToBase64String(hashWithSaltBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashWithSaltBytes;
            try
            {
                hashWithSaltBytes = Convert.FromBase64String(hashedPassword);
            }
            catch (FormatException)
            {
                return false;
            }

            if (hashWithSaltBytes.Length != SaltSize + HashSize)
            {
                return false;
            }

            byte[] saltBytes = hashWithSaltBytes[..SaltSize];
            byte[] hashBytes = hashWithSaltBytes[SaltSize..];

            byte[] actualHashBytes = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithmName.SHA1, HashSize);

            return CryptographicOperations.FixedTimeEquals(hashBytes, actualHashBytes);
        }
    }
}