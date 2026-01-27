using System.Security.Cryptography;
using System.Text;
using Visus.Cuid;

namespace Saturn.Helpers;

internal static class IDHelpers
{
    // 18-character NID/GNID generator
    public static string GenerateID()
    {
        Span<byte> buffer = stackalloc byte[8];
        RandomNumberGenerator.Fill(buffer);
        ulong value = BitConverter.ToUInt64(buffer);
        value %= 1_000_000_000_000_000_000UL;
        return value.ToString("D18");
    }
    
    // pfSessionToken generator
    public static string GenerateToken()
    {
        Cuid2 id = new(32);
        return id.ToString();
    }
}