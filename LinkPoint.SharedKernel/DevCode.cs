using Isopoh.Cryptography.Argon2;
using System.Security.Cryptography;

namespace LinkPoint.SharedKernel;

public static class DevCode
{
    public static string HashPassword(string password) => Argon2.Hash(password);

    public static bool VerifyPassword(string hash, string password) => Argon2.Verify(hash, password);

    public static byte[] RandomBytes(int length) => RandomNumberGenerator.GetBytes(length);

    public static string GenerateToken(int size = 32) => Convert.ToBase64String(RandomBytes(size)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}