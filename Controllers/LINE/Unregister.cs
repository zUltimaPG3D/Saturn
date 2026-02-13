using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Status = Saturn.Messages.AccountLoginResponse.Types.Status;

using Saturn.Helpers;
using Saturn.Messages;
using Saturn.Types;
using System.Text.Json;

namespace Saturn.Controllers.LINE;

[ApiController]
public class Unregister : ControllerBase
{
    [HttpPost("linegames/unregister")]
    public async Task<ActionResult> UnregisterAccount([FromForm] string nid)
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
        
        await DbHelperService.DeleteUserFromNIDAsync(nid);

        JsonObject response = new()
        {
            ["isSuccess"] = true
        };
        
        return RequestHelpers.Json(response);
    }
}
