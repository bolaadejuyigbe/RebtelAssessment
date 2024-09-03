using Library.DataModel.Database;
using Library.Grpc;
using Library.Grpc.Configuration;
using Library.Grpc.Health;
using Library.Grpc.Interceptors;
using Library.Grpc.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc( options =>
{
    options.Interceptors.Add<LoggingAndErrorHandlingInterceptor>();
});
builder.Services.Configure<DbSettings>(options =>
{
    options.DbConnectionString = builder.Configuration.GetValue<string>("DbConnectionString");
});
builder.Services.AddDbContext<LibraryDbContext>((serviceProvider, options) =>
{
    var dbSettings = serviceProvider.GetRequiredService<IOptions<DbSettings>>().Value;
    options.UseSqlServer(dbSettings.DbConnectionString);
});
ModuleConfiguration.ConfigureServices(builder.Services);
var kestrelOptions = builder.Configuration.GetSection(nameof(KestrelConfiguration)).Get<KestrelConfiguration>();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Parse(kestrelOptions!.Ip), kestrelOptions.HttpPort);
    options.Listen(IPAddress.Parse(kestrelOptions.Ip), kestrelOptions.GrpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        // Log the error
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}
// Configure the HTTP request pipeline.
app.MapGrpcService<LibraryServiceImpl>();
app.MapGrpcService<HealthCheck>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
