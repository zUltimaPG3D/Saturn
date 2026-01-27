using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using Saturn.Helpers;
using Saturn.Messages;
using Saturn.Types;

namespace Saturn.Controllers.BeXide.User;

[ApiController]
public class Home : ControllerBase
{
    [HttpPost("api/user/home")]
    public async Task<ActionResult> GetUserHome()
    {
        var data = await Request.ToMessage<UserHomeRequest>();

        var response = new UserHomeResponse
        {
            Head = new CommonResponse()
        };
        
        var user = HttpContext.Items["User"] as ToroUser;
        if (user == null)
        {
            response.Head.Code = 403;
            response.Head.Message = "User with that pfSessionToken doesn't exist";
            
            return RequestHelpers.Protobuf(response);
        }
        
        response.FriendWordList.AddRange(user.FriendWordList);
        return RequestHelpers.Protobuf(response);
    }
}
