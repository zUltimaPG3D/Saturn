using System.Text;

namespace Saturn.Helpers;

public static class Hashes
{
    private const uint Adler32Modulus = 65521;

    public static uint Adler32(byte[] data)
    {
        uint a = 0, b = 0;
        foreach (byte c in data)
        {
            a = (a + c) % Adler32Modulus;
            b = (b + a) % Adler32Modulus;
        }
        return (b << 16) | a;
    }
    
    public static uint Adler32Number(string contents)
    {
        return Adler32(Encoding.UTF8.GetBytes(contents));
    }

    public static string Adler32(string contents)
    {
        return Adler32(Encoding.UTF8.GetBytes(contents)).ToString("X");
    }
}