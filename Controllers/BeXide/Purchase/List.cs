using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using Saturn.Helpers;
using Saturn.Messages;
using Saturn.Types;

namespace Saturn.Controllers.BeXide.Purchase;

[ApiController]
public class List : ControllerBase
{
    [HttpPost("api/purchase/list")]
    public async Task<ActionResult> GetPurchaseList()
    {
        var data = await Request.ToMessage<PurchaseListRequest>();

        var response = new PurchaseListResponse
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
        
        // this is just random data to make it so the coin shop doesn't kill the game
        response.ProductList.Add(new ProductItem()
        {
            ProductId = "first",
            Price = 999.99f,
            Title = "Placeholder",
            Coin = 1,
            Description = "Placeholder",
            Icon = "Coin",
            PurchaseId = "first",
            Limit = 1
        });
        
        return RequestHelpers.Protobuf(response);
    }
}
