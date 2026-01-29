using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Saturn.Helpers;
using Saturn.Types;

namespace Saturn.Controllers;

public class Pages : Controller
{
    [HttpGet("/")]
    public async Task<ActionResult> IndexRedirect()
    {
        return Redirect("/home/");
    }

    [HttpGet("home")]
    public async Task<ActionResult> Home()
    {
        var (scope, service) = DbHelperService.Get();
        int users = await service.IGetUserCount();
        scope.Dispose();
        
        ViewData["UserCount"] = users;
        return View("~/Views/Home/Index.cshtml");
    }

    [HttpGet("home/dashboard")]
    public async Task<ActionResult> Dashboard()
    {
        string token = HttpContext.Request.Cookies.FirstOrDefault(p => p.Key == "token").Value ?? "";
        
        ViewData["Error"] = HttpContext.Request.Cookies.FirstOrDefault(p => p.Key == "error").Value ?? "";
        ViewData["Authenticated"] = !string.IsNullOrEmpty(token) && (await DbHelperService.GetUserFromTokenAsync(token)) != null;
        if (HttpContext.Request.Cookies.Any(p => p.Key == "error"))
        {
            HttpContext.Response.Cookies.Delete("error");
        }
        return View("~/Views/Home/Dashboard.cshtml");
    }
    
    [HttpPost("account/login/withNid")]
    public async Task<ActionResult> LoginWithNID([FromForm] string NID)
    {
        var userWithNID = await DbHelperService.GetUserFromNIDAsync(NID);
        if (userWithNID == null)
        {
            HttpContext.Response.Cookies.Append("error", "No user with this NID exists.");
        }
        else
        {
            string accessToken = userWithNID.Token;
            HttpContext.Response.Cookies.Append("token", accessToken);
        }
        
        return Redirect("/home/dashboard");
    }
    
    private const string ActionSuccess = "Done, you can start the game again";
    
    [HttpPost("account/controlAction")]
    public async Task<ActionResult> ControlAction([FromForm] string action)
    {
        string token = HttpContext.Request.Cookies.FirstOrDefault(p => p.Key == "token").Value ?? "";
        
        if (string.IsNullOrEmpty(token))
        {
            HttpContext.Response.Cookies.Append("error", "The given token is empty.");
            return Redirect("/home/dashboard");
        
        }
        
        var userWithToken = await DbHelperService.GetUserFromTokenAsync(token);
        if (userWithToken == null)
        {
            HttpContext.Response.Cookies.Append("error", "No user with this NID exists.");
            return Redirect("/home/dashboard");
        }
        
        switch (action)
        {
            case "maxStars":
            case "maxHearts":
                var playerInfoProp = userWithToken.PropertyList.FirstOrDefault(p => p.Key == "PlayerInfo");
                if (playerInfoProp == null)
                {
                    HttpContext.Response.Cookies.Append("error", "This account doesn't have PlayerInfo yet!");
                    break;
                }
                var playerInfo = JsonNode.Parse(playerInfoProp.Value);
                if (playerInfo == null)
                {
                    HttpContext.Response.Cookies.Append("error", "Failed to parse PlayerInfo!");
                    break;
                }
                
                if (action == "maxStars") playerInfo["resourceCount"] = 999_999;
                if (action == "maxHearts") playerInfo["HeartCount"] = 5;
                playerInfoProp.Value = playerInfo.ToJsonString();
                
                userWithToken.PropertyList = [.. userWithToken.PropertyList.Select(p => p.Key == "PlayerInfo" ? playerInfoProp : p)];
                await userWithToken.UpdateAsync();
                HttpContext.Response.Cookies.Append("error", ActionSuccess);
                break;
            case "giveCoins":
                userWithToken.Coins += 10_000;
                userWithToken.PropertyList.FirstOrDefault(p => p.Key == "ServerData.Item.Coin")?.Value = userWithToken.Coins.ToString();
                await userWithToken.UpdateAsync();
                HttpContext.Response.Cookies.Append("error", ActionSuccess);
                break;
        }
        
        return Redirect("/home/dashboard");
    }
}
