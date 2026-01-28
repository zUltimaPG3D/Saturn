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
        
        foreach (string path in Directory.GetFiles(Path.Combine(GameInfo.StaticContentPath, "master01")))
        {
            string name = Path.GetFileNameWithoutExtension(path);
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(path);
            byte[] hashBytes = MD5.HashData(bytes);
            string hash = Convert.ToHexString(hashBytes);

            response.Data.Add(new Messages.MasterData()
            {
                Name = name,
                Hash = hash,
                Url = GameInfo.MakeURL($"/master01/{name}.json"),
                Size = bytes.Length
            });
        }

        return RequestHelpers.Protobuf(response);
    }
}
