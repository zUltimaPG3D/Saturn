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

### For testing
- Simply run this command:
```bash
$ dotnet run
```
- The server is now running!

To edit the running port, you'll have to edit `Properties/launchSettings.json`.

### For production
- Run this command:
```bash
$ dotnet publish
```

You will get an executable called `Saturn` (alongside a bunch of other files) inside of `bin/Release/net10.0/publish`.

* If you're running the server on **the same machine as where you built it**, you can simply run `./Saturn` inside the folder.

* If you're running the server on **a separate machine**, you'll have to copy the *entire* `publish` folder and *all its contents* in order for it to run correctly.

To edit the running port (which by default is 5000), you'll have to run Saturn like this:
```bash
$ ./Saturn --urls http://0.0.0.0:15154/
# ... where 15154 is the port it will run on
```

---
