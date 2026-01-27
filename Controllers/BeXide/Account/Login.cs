using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Saturn.Helpers;
using Saturn.Messages;
using Saturn.Types;

namespace Saturn.Controllers.BeXide.Account;

[ApiController]
public class Login : ControllerBase
{
    [HttpPost("api/account/login")]
    public async Task<ActionResult> AccountLogin()
    {
        var data = await Request.ToMessage<AccountLoginRequest>();

        var response = new AccountLoginResponse
        {
            Head = new CommonResponse()
        };

        data.DeviceId = BXCrypt.Decrypt(data.DeviceId);
        if (!string.IsNullOrEmpty(data.PublicId)) data.PublicId = BXCrypt.Decrypt(data.PublicId); // this is useless for now because i don't know what this is even used for
        
        ToroUser? user = await DbHelperService.GetUserFromNIDAsync(data.DeviceId);
        if (user == null)
        {
            response.Head.Code = 403;
            response.Head.Message = "User with that NID doesn't exist";
            
            return RequestHelpers.Protobuf(response);
        }
        
        response.Head.Code = 0;
        response.Head.AccessToken = user.Token;
        response.PublicId = user.PublicID.ToString();
        response.Master = 0;
        response.Resource = 0;
        response.IsSavedataExpired = 0;
        response.Status = user.UserType;
        response.RedirectHost = "";
        response.PlayerSummary = new Summary();
        response.ExpireStatus = ExpireStatus.None;
        return RequestHelpers.Protobuf(response);
    }
}
