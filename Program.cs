using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Saturn.Middlewares;
using Saturn.Types;

internal partial class Program
{
    public static Random RNG = new();
    public static WebApplication? Application;
        
    private static readonly FileExtensionContentTypeProvider Provider = new()
    {
        Mappings =
        {
            [".cpk"] = "application/octet-stream; charset=binary",
            [".hash"] = "text/plain; charset=utf-8",
            [".bundle"] = "application/octet-stream; charset=binary",
        }
    };
    
    private static void UseStaticFiles(WebApplication app, string path)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(GameInfo.StaticContentPath, path)),
            RequestPath = $"/{path}",
            ContentTypeProvider = Provider
        });
    }
    
    private static async Task Main(string[] args)
    {
        GameInfo.Initialize();
        
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        
        builder.Services.AddDbContext<GameDbContext>(options => options.UseSqlite("Data Source=game.db"));
        builder.Services.AddScoped<DbHelperService>();

        var app = builder.Build();
        
        var downloadAliases = new Dictionary<string,string>
        {
            ["/files/3472da8f5f0ecdc15ae131ffc321a860"] = "/files/additionalawb.cpk",
            ["/files/e4eca25358ed8d8c4619801aba79b5d2"] = "/files/en_US_archive.zip",
            ["/files/53ef97cfb6cf245f6f30f12ec1ca7a3d"] = "/files/ko_KR_archive.zip",
            ["/files/751310b495400082c71e04ea15f3e2b5"] = "/files/zh_CN_archive.zip",
            ["/files/e04faecf811d5a89c23bdb7a443cae56"] = "/files/zh_TW_archive.zip",
        };
        
        app.Use(async (context, next) =>
        {
            if (downloadAliases.TryGetValue(context.Request.Path, out var target))
            {
                context.Request.Path = target;
            }
        
            await next();
        });
        
        app.Use(async (context, next) =>
        {
            var request = context.Request;

            static string Normalize(string s) => AntiLeadingSlash().Replace(s ?? "", "/");
        
            request.Path = Normalize(request.Path.Value!);
            request.PathBase = Normalize(request.PathBase.Value!);
        
            // Console.WriteLine($"{context.Request.Method} request to {context.Request.GetDisplayUrl()}");
            await next();
        });
        
        UseStaticFiles(app, "files");
        UseStaticFiles(app, "assets");
        UseStaticFiles(app, "master01");
        UseStaticFiles(app, "site");
        
        app.UseMiddleware<ToroUserMiddleware>();
        app.UseRouting();
        app.MapRazorPages();
        app.MapControllers();
        app.MapDefaultControllerRoute();

        Application = app;
        
        await DbHelperService.Initialize();
        app.Run();
    }

    [GeneratedRegex("/{2,}")]
    private static partial Regex AntiLeadingSlash();
}