using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Saturn.Types;

namespace Saturn.Controllers.Neptune;

[ApiController]
public class NeptuneInfo : ControllerBase
{
    [HttpPost("api/langCulture/game/useList")]
    public async Task<ActionResult> LanguageUseList()
    {
        byte[] data = await System.IO.File.ReadAllBytesAsync("Static/langUseList.json");
        return File(data, "application/json; charset=utf-8");
    }
    
    [HttpPost("api/gameServer/list")]
    public async Task<ActionResult> GameServerList()
    {
        byte[] data = await System.IO.File.ReadAllBytesAsync("Static/gameServerList.json");
        return File(data, "application/json; charset=utf-8");
    }
    
    [HttpPost("api/api_version/getClientVersionInfo")]
    public async Task<ActionResult> ClientVersionInfo()
    {
        JsonObject json = new()
        {
            ["isSuccess"] = true,
            ["data"] = new JsonObject()
            {
                ["client_version_status"] = GameInfo.GameStatus.ToString(),
                ["server_addr"] = GameInfo.BaseURL,
                ["patch_addr"] = GameInfo.BaseURL,
                ["countryInfo"] = new JsonObject()
                {
                    ["countryCd"] = "US",
                    ["gdprTargetYn"] = "N",
                },
                ["countryTermsInfos"] = new JsonArray()
                {
                    new JsonObject()
                    {
                        ["cd"] = "TERMS_OF_SERVICES",
                        ["required"] = true,
                        ["url"] = GameInfo.BaseURL
                    },
                    new JsonObject()
                    {
                        ["cd"] = "PRIVACY_POLICY",
                        ["required"] = true,
                        ["url"] = GameInfo.BaseURL
                    },
                    new JsonObject()
                    {
                        ["cd"] = "PRIVACY_POLICY_FULLTEXT",
                        ["required"] = true,
                        ["url"] = GameInfo.BaseURL
                    },
                    new JsonObject()
                    {
                        ["cd"] = "USE_OF_PUSH_NOTIFICATIONS",
                        ["required"] = false,
                        ["url"] = GameInfo.BaseURL
                    }
                },
                ["maintenance_msg"] = GameInfo.MaintenanceMessage,
                ["guest_mode_on_yn"] = "Y",
                ["applied_white_yn"] = "Y",
                ["customValue"] = "",
                ["out_link_url"] = GameInfo.BaseURL
            }
        };
        return File(Encoding.UTF8.GetBytes(json.ToJsonString()), "application/json; charset=utf-8");
    }
}
