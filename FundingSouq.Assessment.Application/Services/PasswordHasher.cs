using System.Security.Cryptography;
using FundingSouq.Assessment.Core.Interfaces;

namespace FundingSouq.Assessment.Application.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }
    
    public bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split('-', 2);
        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);

        var testHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, hash.Length);

        return CryptographicOperations.FixedTimeEquals(hash, testHash);
    }
}