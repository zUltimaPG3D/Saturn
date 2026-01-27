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
public class MasterData : ControllerBase
{
    [HttpPost("api/account/masterdata")]
    public async Task<ActionResult> FetchMasterData()
    {
        var data = await Request.ToMessage<MasterDataRequest>();

        var response = new MasterDataResponse
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
        
        return RequestHelpers.Protobuf(response);
    }
}
