using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Saturn.Helpers;

public static partial class BXCrypt
{
    public static void Initialize(string key, string iv, string head)
    {
        m_Key = key;
        m_IV = iv;
        m_plainTextHead = head;
    }
    
    public static string Encrypt(string uid)
    {
        StringBuilder sb = new(m_plainTextHead);
        sb.Append(uid);
        using var rijn = RijndaelManaged.Create();
        rijn.Padding = PaddingMode.Zeros;
        rijn.Mode = CipherMode.CBC;
        rijn.KeySize = 256;
        rijn.BlockSize = 128;
        ICryptoTransform encryptor = rijn.CreateEncryptor(Encoding.UTF8.GetBytes(m_Key), Encoding.UTF8.GetBytes(m_IV));
        MemoryStream memoryStream = new();
        CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(Encoding.UTF8.GetBytes(sb.ToString()));
        cryptoStream.FlushFinalBlock();
        return Convert.ToBase64String(memoryStream.ToArray());
    }
    public static string Decrypt(string src)
    {
        using var rijn = RijndaelManaged.Create();
        rijn.Padding = PaddingMode.Zeros;
        rijn.Mode = CipherMode.CBC;
        rijn.KeySize = 256;
        rijn.BlockSize = 128;
        ICryptoTransform decryptor = rijn.CreateDecryptor(Encoding.UTF8.GetBytes(m_Key), Encoding.UTF8.GetBytes(m_IV));
        MemoryStream memoryStream = new(Convert.FromBase64String(src));
        CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
        StreamReader streamReader = new(cryptoStream);
        string decrypted = streamReader.ReadToEnd();
        if (decrypted.Contains(m_plainTextHead, StringComparison.CurrentCultureIgnoreCase))
        {
            decrypted = decrypted[m_plainTextHead.Length..];
            decrypted = RegexDecryptReplace().Replace(decrypted, "");
        }
        memoryStream.Dispose();
        streamReader.Dispose();
        cryptoStream.Dispose();
        return decrypted;
    }
    
    public static string m_Key = "AehiP1hohphe4ith6ievoh4saht3aeca";
    public static string m_IV = "eiGadaegungoo0gu";
    public static string m_plainTextHead = "bx70test1";

    [GeneratedRegex("[\u0000-\u0019+/]")]
    private static partial Regex RegexDecryptReplace();
}