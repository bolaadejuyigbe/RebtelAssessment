using Library.Api.Interceptors;
using Library.Api.Middleware;
using Library.Api.Policies;
using Library.Grpc.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ClientDeadlineInterceptor>();
builder.Services.AddScoped<ResilientPolicy>();
builder.Services.AddGrpcClient<LibraryService.LibraryServiceClient>(x =>
{
    var serviceAddress = builder.Configuration.GetValue<string>("Grpc:LibraryServiceUrl")
                                  ?? throw new MissingFieldException("LibraryServiceUrl not set as a configuration item");

    x.Address = new Uri(serviceAddress);
}).AddInterceptor<ClientDeadlineInterceptor>();
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
