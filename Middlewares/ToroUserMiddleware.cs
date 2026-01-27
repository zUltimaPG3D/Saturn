using Saturn.Types;

namespace Saturn.Middlewares;

internal class ToroUserMiddleware
{
    private readonly RequestDelegate _next;

    public ToroUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            string token = authHeader["Bearer ".Length..];
            var user = await DbHelperService.GetUserFromTokenAsync(token);
            if (user != null)
            {
                context.Items["User"] = user;
            }
        }
        else
        {
            context.Items["User"] = null;
        }

        await _next(context);
    }
}
