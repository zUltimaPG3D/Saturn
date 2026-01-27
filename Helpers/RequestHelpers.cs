using System.Text;
using System.Text.Json.Nodes;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;

namespace Saturn.Helpers;

internal static class RequestHelpers
{   
    public static FileContentResult Json(JsonNode json)
    {
        return new FileContentResult(Encoding.UTF8.GetBytes(json.ToJsonString()), "application/json; charset=utf-8");
    }
    
    public static FileContentResult Protobuf(IMessage msg)
    {
        return new FileContentResult(msg.ToByteArray(), "application/x-protobuf");
    }
}