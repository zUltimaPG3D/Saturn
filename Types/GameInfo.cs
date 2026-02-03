using System.Text;

namespace Saturn.Types;

internal enum GameStatus
{
    ONLINE,
    OFFLINE,
    MAINTENANCE,
}

internal static class GameInfo
{
    public static GameStatus GameStatus = GameStatus.ONLINE;
    public static string IP = "onsen.pixelgun.plus";
    public static int Port = 443;
    public static bool IsHTTPS = false;
    public static string MaintenanceMessage = "";
    public static string StaticContentPath = "";

    public static void Initialize()
    {
        StaticContentPath = Path.Combine(Environment.CurrentDirectory, "Static", "Content");
    }

    public static string BaseURL
    {
        get
        {
            return MakeURL("/");
        }
    }
    
    public static string MakeURL(string path)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append(IsHTTPS ? "https" : "http");
        stringBuilder.Append("://");
        stringBuilder.Append(IP);
        if (Port != 80 && Port != 443)
        {
            stringBuilder.Append(':');
            stringBuilder.Append(Port);
        }
        stringBuilder.Append(path);
        return stringBuilder.ToString();
    }
}