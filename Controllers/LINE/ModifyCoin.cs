using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Saturn.Helpers;
using Saturn.Types;

namespace Saturn.Controllers.LINE;

[ApiController]
public class ModifyCoin : ControllerBase
{
    [HttpPost("linegames/modifycoin")]
    public async Task<ActionResult> SetCoins([FromForm] string nid, [FromForm] string? rewardCount, [FromForm] string? rewardItem)
    {
        if (string.IsNullOrEmpty(nid))
        {
            JsonObject json = new()
            {
                ["isSuccess"] = false,
                ["msg"] = "Invalid NID"
            };

            return RequestHelpers.Json(json);
        }

        nid = BXCrypt.Decrypt(nid);
        ToroUser? user = await DbHelperService.GetUserFromNIDAsync(nid);
        if (user == null)
        {
            JsonObject json = new()
            {
                ["isSuccess"] = false,
                ["msg"] = "Invalid user from NID"
            };

            return RequestHelpers.Json(json);
        }
        
        if (!string.IsNullOrEmpty(rewardCount))
        {
            rewardCount = BXCrypt.Decrypt(rewardCount);
            if (!int.TryParse(rewardCount, out int coinCount))
            {
                JsonObject json = new()
                {
                    ["isSuccess"] = false,
                    ["msg"] = "Invalid coin count"
                };
    
                return RequestHelpers.Json(json);
            }
        
            user.Coins += 50 + coinCount;
            await user.UpdateAsync();
        }
        
        // TODO: implement coin gifts
        
        JsonObject data = new()
        {
            ["asset"] = new JsonObject()
            {
                ["coin"] = user.Coins
            }
        };

        JsonObject response = new()
        {
            ["isSuccess"] = true,
            ["data"] = BXCrypt.Encrypt(data.ToJsonString())
        };
        
        return RequestHelpers.Json(response);
    }
}
