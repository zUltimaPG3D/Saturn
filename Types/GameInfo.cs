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
    public static string IP = "192.168.1.20";
    public static int Port = 15154;
    public static bool IsHTTPS = false;

    public static string MaintenanceMessage = "";

    public static string BaseURL
    {
        get
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
            stringBuilder.Append('/');
            return stringBuilder.ToString();
        }
    }
}