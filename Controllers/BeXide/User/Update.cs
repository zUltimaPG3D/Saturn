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
public class Update : ControllerBase
{
    [HttpPost("api/user/update")]
    public async Task<ActionResult> UpdateUser()
    {
        var data = await Request.ToMessage<UserUpdateRequest>();

        var response = new UserInfoResponse
        {
            Head = new CommonResponse()
        };

        if (HttpContext.Items["User"] is not ToroUser user)
        {
            response.Head.Code = 403;
            response.Head.Message = "User with that pfSessionToken doesn't exist";

            return RequestHelpers.Protobuf(response);
        }

        user.PropertyList.Clear();
        user.PropertyList.AddRange(data.PropertyList);
        await user.UpdateAsync();
        
        response.PropertyList.AddRange(user.PropertyList);
        return RequestHelpers.Protobuf(response);
    }
}
