using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Saturn.Helpers;
using Saturn.Messages;
using Status = Saturn.Messages.AccountLoginResponse.Types.Status;

namespace Saturn.Types;

public class ToroUser
{
    [Key] public ulong PublicID { get; set; }
    public string DeviceID { get; set; }
    public string NID { get; set; }
    public string GNID { get; set; }
    public string Token { get; set; }
    
    public bool AgreedToPush { get; set; }
    public bool AgreedToTerms { get; set; }
    public long PushAgreeTime { get; set; }
    public long TermsAgreeTime { get; set; }
    
    public bool IsNew { get; set; } = true;
    public int Coins { get; set; } = 0;
    public List<UserProperty> PropertyList { get; set; } = [];
    
    public Status UserType { get; set; } = Status.Normal;
    
    public List<string> FriendWordList = [];
    
    public SupportAccount ToSupportAccount(bool isSelf = false)
    {
        var acc = new SupportAccount
        {
            UserId = (long)PublicID,
            PublicId = PublicID.ToString(),
            PlayerName = PropertyList.FirstOrDefault(p => p.Key == "ServerData.nickname")?.Value ?? "Player",
            PlayerData = PropertyList.FirstOrDefault(p => p.Key == "PlayerData")?.Value ?? "{}",
            Status = isSelf ? FriendStatus.Own : FriendStatus.None
        };

        return acc;
    }
}