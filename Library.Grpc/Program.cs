using Library.Grpc;
using Library.Grpc.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

var kestrelOptions = builder.Configuration.GetSection(nameof(KestrelConfiguration)).Get<KestrelConfiguration>();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Parse(kestrelOptions!.Ip), kestrelOptions.HttpPort);
    options.Listen(IPAddress.Parse(kestrelOptions.Ip), kestrelOptions.GrpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
var startup = new StartupExtension(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();
