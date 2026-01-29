using Microsoft.EntityFrameworkCore;
using Saturn.Helpers;
using Saturn.Messages;

namespace Saturn.Types;

public class DbHelperService(GameDbContext db)
{
    private readonly GameDbContext _db = db;
    
    public static DbHelperService Get()
    {
        using var scope = Program.Application!.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<DbHelperService>();
        return service;
    }
    
    public static async Task<ToroUser?> GetUserFromTokenAsync(string token)
    {
        using var scope = Program.Application!.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<DbHelperService>();
        var user = await service.IGetUserFromTokenAsync(token);
        return user;
    }
    
    public static async Task<ToroUser?> GetUserFromNIDAsync(string nid)
    {
        using var scope = Program.Application!.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<DbHelperService>();
        var user = await service.IGetUserFromNIDAsync(nid);
        return user;
    }
    
    public static async Task<List<SupportAccount>> GetAllSupportAccounts(ulong myId)
    {
        using var scope = Program.Application!.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<DbHelperService>();
        var users = await service.IGetAllUsers();
        return [.. users.Select(u => u.ToSupportAccount(myId == u.PublicID))];
    }
    
    public static async Task<ToroUser> CreateNewOrGet(string id)
    {
        using var scope = Program.Application!.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<DbHelperService>();
        var user = await service.ICreateNewOrGet(id);
        return user;
    }

    public async Task<ToroUser?> IGetUserFromTokenAsync(string token)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Token == token);
        return user;
    }
    
    public async Task<ToroUser?> IGetUserFromNIDAsync(string nid)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.NID == nid);
        return user;
    }

    public async Task UpdateAsync(ToroUser user)
    {
        var exists = await _db.Users.AnyAsync(u => u.DeviceID == user.DeviceID);

        if (exists)
            _db.Users.Update(user);
        else
            _db.Users.Add(user);
        
        await _db.SaveChangesAsync();
    }
    
    public async Task<ToroUser> ICreateNewOrGet(string id)
    {
        var exists = await _db.Users.AnyAsync(u => u.DeviceID == id);
        
        if (exists)
        {
            var getUser = await _db.Users.FirstOrDefaultAsync(u => u.DeviceID == id);
            return getUser;
        }
        
        ToroUser user = new()
        {
            DeviceID = id,
            NID = IDHelpers.GenerateID(),
            GNID = IDHelpers.GenerateID(),
            Token = IDHelpers.GenerateToken(),
            AgreedToPush = false,
            AgreedToTerms = false,
            PushAgreeTime = 0,
            TermsAgreeTime = 0,
            IsNew = true,
            Coins = 0,
            PropertyList = [],
            FriendWordList = [],
            UserType = Status.Normal
        };
        return user;
    }
    
    public async Task<DbSet<ToroUser>> IGetAllUsers()
    {
        return _db.Users;
    }
}
