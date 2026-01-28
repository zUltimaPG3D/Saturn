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

        var lookup = data.PropertyList.ToDictionary(x => x.Key);
        var merged = user.PropertyList
            .Select(p => lookup.TryGetValue(p.Key, out var replacement) ? replacement : p)
            .ToList();
        merged.AddRange(data.PropertyList.Where(d => !user.PropertyList.Any(u => u.Key == d.Key)));
        user.PropertyList = merged;
        await user.UpdateAsync();
        
        response.PropertyList.AddRange(user.PropertyList);
        return RequestHelpers.Protobuf(response);
    }
}
