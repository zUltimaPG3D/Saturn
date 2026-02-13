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

        if (HttpContext.Items["User"] is not ToroUser user)
        {
            response.Head.Code = 403;
            response.Head.Message = "User with that pfSessionToken doesn't exist";

            return RequestHelpers.Protobuf(response);
        }

        if (data.PublicId == "random")
        {
            var users = await DbHelperService.GetRandomSupportAccounts(user.PublicID);
            response.Data.AddRange(users);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(data.PublicId))
            {
                var userWithId = await DbHelperService.GetOneSupportAccount(user.PublicID, data.PublicId);
                if (userWithId != null)
                {
                    response.Data.Add(userWithId);
                }
            }
        }
        
        return RequestHelpers.Protobuf(response);
    }
}
