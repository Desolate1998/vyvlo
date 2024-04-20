using System.Security.Cryptography;

namespace Infrastructure.Core.PasswordHelper;

public static class PasswordHelper
{
    public static string HashPassword(string password, byte[] salt)
    {
        using Rfc2898DeriveBytes pbkdf2 = new(password, salt, 10_000, HashAlgorithmName.SHA256);
        return Convert.ToBase64String(pbkdf2.GetBytes(32));
    }

    public static byte[] GenerateSalt()
    {
        var salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    public static bool ValidLoginPassword(string password, string salt, string hash)
    {
        return HashPassword(password, Convert.FromBase64String(salt)) == hash;
    }
}