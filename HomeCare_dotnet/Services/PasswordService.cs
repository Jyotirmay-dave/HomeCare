using System;
using System.Security.Cryptography;

namespace HomeCare_dotnet.Services;

public class PasswordService : IPasswordService
{
    public byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public string HashPassword(string password, byte[] salt, int iterations = 10000)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
        {
            return Convert.ToBase64String(pbkdf2.GetBytes(32));
        }
    }
}
