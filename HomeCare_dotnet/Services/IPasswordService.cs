using System;

namespace HomeCare_dotnet.Services;

public interface IPasswordService
{
    byte[] GenerateSalt(int size = 16);
    string HashPassword(string password, byte[] salt, int iterations = 10000);
}
