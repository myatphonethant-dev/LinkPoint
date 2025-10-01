using Isopoh.Cryptography.Argon2;

namespace LinkPoint.SharedKernel;

public static class DevCode
{
    public static string HashPassword(string password)
    {
        return Argon2.Hash(password);
    }

    public static bool VerifyPassword(string hash, string password)
    {
        return Argon2.Verify(hash, password);
    }
}