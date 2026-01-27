using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using Saturn.Helpers;
using Saturn.Messages;
using Saturn.Types;

namespace Saturn.Controllers.BeXide.Friends;

[ApiController]
public class List : ControllerBase
{
    [HttpPost("api/friend/list")]
    public async Task<ActionResult> GetFriendsList()
    {
        var data = await Request.ToMessage<FriendListRequest>();

        var response = new FriendListResponse
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
        
        // TODO: implement friends
        
        var users = await DbHelperService.GetAllSupportAccounts(user.PublicID);
        
        response.Data.AddRange(users);
        
        return RequestHelpers.Protobuf(response);
    }
}
