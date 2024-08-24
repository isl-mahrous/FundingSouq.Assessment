using System.Security.Cryptography;
using FundingSouq.Assessment.Core.Interfaces;

namespace FundingSouq.Assessment.Application.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    /// <inheritdoc />
    public string Hash(string password)
    {
        // Generate a random salt
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        // Derive a hash from the password and salt using PBKDF2
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        // Return the hash and salt as a combined string
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }
    
    /// <inheritdoc />
    public bool Verify(string password, string passwordHash)
    {
        // Split the stored hash into its hash and salt components
        var parts = passwordHash.Split('-', 2);
        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);

        // Recompute the hash using the provided password and the stored salt
        var testHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, hash.Length);

        // Compare the stored hash and the newly computed hash in constant time
        return CryptographicOperations.FixedTimeEquals(hash, testHash);
    }
}
