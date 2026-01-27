using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Saturn.Helpers;
using Saturn.Messages;
using Saturn.Types;

namespace Saturn.Controllers.LINE;

[ApiController]
public class LINELogin : ControllerBase
{
    [HttpPost("linegames/getnid")]
    public async Task<ActionResult> GetNID([FromForm] string pfSessionToken)
    {
        if (string.IsNullOrEmpty(pfSessionToken))
        {
            JsonObject json = new()
            {
                ["isSuccess"] = false,
                ["msg"] = "Invalid pfSessionToken"
            };

            return RequestHelpers.Json(json);
        }
        
        pfSessionToken = BXCrypt.Decrypt(pfSessionToken);

        ToroUser? user = await DbHelperService.GetUserFromTokenAsync(pfSessionToken);
        if (user == null)
        {
            JsonObject json = new()
            {
                ["isSuccess"] = false,
                ["msg"] = "Invalid user from pfSessionToken"
            };

            return RequestHelpers.Json(json);
        }
        
        JsonObject data = new()
        {
            ["gnidStatus"] = user.UserType == Status.Block ? "BLOCK" : "OK",
            ["blockReason"] = "Unspecified",
            ["countryCreated"] = "US",
            ["nid"] = user.NID,
            ["gnid"] = user.GNID,
        };

        JsonObject response = new()
        {
            ["isSuccess"] = true,
            ["data"] = BXCrypt.Encrypt(data.ToJsonString())
        };
        
        return RequestHelpers.Json(response);
    }
}
