<h1 align="center">
  <a href="https://github.com/zUltimaPG3D/Saturn/"><img src="Static/Content/site/logo.png" alt="Saturn" width="400"></a>
</h1>

<h4 align="center">(Almost) Fully functional Toro and Friends: Onsen Town backend recreation (LINE, NeptuneSDK and BeXide APIs)</h4>

<p align="center">
  Contents:
  <a href="#requirements">Requirements</a> •
  <a href="#setup">Setup</a>
</p>

## Requirements
[.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

[EFCore](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

## Setup
- Clone the repository
- Run these commands in the repository's folder:
```bash
$ dotnet restore
$ dotnet ef database update
```
- Edit `Types/GameInfo.cs` to use the right IP and port
  - If you don't want to use a defined port (for example if you're using a domain that's proxied to the running server) you can set `Port` to 80 or 433.
  - Make sure to change `IsHTTPS` accordingly. Using cleartext HTTP on Android requires further setup
- Run this command:
```bash
$ dotnet run
```
- The server is now running!

To edit the running port, you'll have to edit `Properties/launchSettings.json`.

---
