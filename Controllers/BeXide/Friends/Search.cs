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
public class Search : ControllerBase
{
    [HttpPost("api/friend/search")]
    public async Task<ActionResult> SearchForFriends()
    {
        var data = await Request.ToMessage<FriendSearchRequest>();

        var response = new FriendSearchResponse
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
        
        if (data.PublicId == "random")
        {
            // var users = await DbHelperService.GetAllSupportAccounts(user.PublicID);
            // response.Data.AddRange(users);
        }
        else
        {
            var users = await DbHelperService.GetAllSupportAccounts(user.PublicID);
            var userWithId = users.FirstOrDefault(u => u.PublicId == data.PublicId);
            if (userWithId != null)
            {
                response.Data.Add(userWithId);
            }
        }
        
        return RequestHelpers.Protobuf(response);
    }
}
