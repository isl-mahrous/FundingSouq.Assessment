namespace FundingSouq.Assessment.Core.Interfaces;

/// <summary>
/// Provides methods for hashing passwords and verifying hashed passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes the specified password using a secure hashing algorithm.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A hashed representation of the password.</returns>
    string Hash(string password);

    /// <summary>
    /// Verifies the specified password against the provided password hash.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="passwordHash">The hash to compare against.</param>
    /// <returns><c>true</c> if the password matches the hash; otherwise, <c>false</c>.</returns>
    bool Verify(string password, string passwordHash);
}
