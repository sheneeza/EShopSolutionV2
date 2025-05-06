using System.Security.Cryptography;
using AuthenticationAPI.ApplicationCore.Contracts.Repositories;

namespace AuthenticationAPI.Infrastructure.Repositories;

public class PasswordHelper : IPasswordHelper
{
    private const int SaltSize   = 16;   // 128-bit
    private const int KeySize    = 32;   // 256-bit
    private const int Iterations = 10000;

    public string GenerateSalt()
    {
        var saltBytes = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    public string Hash(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password, saltBytes, Iterations, HashAlgorithmName.SHA256);

        var key = pbkdf2.GetBytes(KeySize);
        return Convert.ToBase64String(key);
    }

    public bool Verify(string password, string hash, string salt)
    {
        var computed = Hash(password, salt);
        return computed == hash;
    }
}