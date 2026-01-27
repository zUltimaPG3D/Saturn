using Google.Protobuf;
using Saturn.Types;

namespace Saturn.Helpers;

internal static class Extensions
{
    public static Task UpdateAsync(this ToroUser user)
    {
        using var scope = Program.Application!.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<DbHelperService>();
        return service.UpdateAsync(user);
    }
    
    public static async Task<T> ToMessage<T>(this HttpRequest Request) where T : IMessage<T>, new()
    {
        using var ms = new MemoryStream();
        await Request.Body.CopyToAsync(ms);
        ms.Position = 0;
        var parser = new MessageParser<T>(() => new T());
        return parser.ParseFrom(ms);
    }
}