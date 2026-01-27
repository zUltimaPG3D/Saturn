using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Saturn.Helpers;
using Saturn.Types;

namespace Saturn.Controllers.Neptune;

[ApiController]
public class NeptuneLogin : ControllerBase
{
    [HttpPost("api/v1/login/guest/getLoginToken")]
    public async Task<ActionResult> GetLoginToken([FromForm] string platformUserId)
    {
        if (string.IsNullOrEmpty(platformUserId))
        {
            JsonObject json = new()
            {
                ["isSuccess"] = false,
                ["msg"] = "Invalid platformUserId"
            };

            return RequestHelpers.Json(json);
        }

        ToroUser user = await DbHelperService.CreateNewOrGet(platformUserId);

        JsonObject response = new()
        {
            ["success"] = true,
            ["data"] = new JsonObject()
            {
                ["newGnidYn"] = user.IsNew ? "Y" : "N",
                ["gnidHash"] = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(user.GNID))),
                ["pfSessionToken"] = user.Token,
                ["countryCreated"] = "US",
                ["policyAgreeInfo"] = new JsonObject()
                {
                    ["termsAgreeUnixTS"] = user.TermsAgreeTime,
                    ["privacyAgreeUnixTS"] = user.TermsAgreeTime,
                    ["ageCheckCompletedUnixTS"] = user.TermsAgreeTime,
                    ["privacyTransferAgreeUnixTS"] = user.TermsAgreeTime,
                    ["nightPushAgreeYn"] = user.AgreedToPush ? "Y" : "N",
                    ["nightPushAgreeUnixTS"] = user.PushAgreeTime,
                    ["pushAgreeYn"] = user.AgreedToPush ? "Y" : "N",
                    ["pushAgreeUnixTS"] = user.PushAgreeTime,
                    ["needAgreePushYn"] = !user.AgreedToPush ? "Y" : "N",
                    ["needReAgreePolicyYn"] = !user.AgreedToTerms ? "Y" : "N"
                },
                ["linkedPlatformIdList"] = new JsonArray()
                {
                    99
                }
            }
        };
        
        user.IsNew = false;
        await user.UpdateAsync();
        return RequestHelpers.Json(response);
    }
    
    [HttpPost("api/policy/v2/nid/agree/request/byPfSessionToken/forClient")]
    public async Task<ActionResult> AgreeToPolicy([FromForm] string pfSessionToken)
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
        
        user.AgreedToTerms = true;
        user.TermsAgreeTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        await user.UpdateAsync();

        JsonObject response = new()
        {
            ["success"] = true,
            ["isSuccess"] = true,
        };
        return RequestHelpers.Json(response);
    }
    
    [HttpPost("api/v1/push/token/register/forClient")]
    public async Task<ActionResult> RegisterPushToken([FromForm] string pfSessionToken, [FromForm] string pushToken)
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
        
        user.AgreedToPush = true;
        user.PushAgreeTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        await user.UpdateAsync();

        JsonObject response = new()
        {
            ["isSuccess"] = true,
            ["msg"] = "Successfully registered push token",
            ["success"] = true,
            ["data"] = new JsonObject()
            {
                ["pushToken"] = pushToken
            },
        };
        return RequestHelpers.Json(response);
    }
}
