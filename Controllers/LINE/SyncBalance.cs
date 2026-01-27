using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Saturn.Helpers;
using Saturn.Types;

namespace Saturn.Controllers.LINE;

[ApiController]
public class SyncBalance : ControllerBase
{
    [HttpPost("linegames/SyncBalance")]
    public async Task<ActionResult> FetchBalance([FromForm] string nid, [FromForm] string gnid)
    {
        if (string.IsNullOrEmpty(nid) || string.IsNullOrEmpty(gnid))
        {
            JsonObject json = new()
            {
                ["isSuccess"] = false,
                ["msg"] = "Invalid NID or GNID"
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
