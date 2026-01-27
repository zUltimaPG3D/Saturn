using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using Saturn.Helpers;
using Saturn.Messages;
using Saturn.Types;

namespace Saturn.Controllers.BeXide.Presents;

[ApiController]
public class List : ControllerBase
{
    [HttpPost("api/present/list")]
    public async Task<ActionResult> GetPresentList()
    {
        var data = await Request.ToMessage<PresentListRequest>();

        var response = new PresentListResponse
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
        
        // TODO: implement gifting
        
        return RequestHelpers.Protobuf(response);
    }
}
