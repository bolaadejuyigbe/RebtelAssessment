using Grpc.Core;
using Grpc.Net.Client;
using Library.DataModel.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Polly;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Library.DataModel.Models;

namespace LibraryService.Test
{
    public abstract class GrpcTestFixture<TStartup> : IDisposable where TStartup : class
    {
        protected const string EnvoyProxyErrorMessage = "upstream connect error or disconnect/reset before headers";

        protected readonly TimeSpan[] RetrySpans = new TimeSpan[5]
        {
        TimeSpan.FromMilliseconds(500.0),
        TimeSpan.FromSeconds(1.0),
        TimeSpan.FromSeconds(2.0),
        TimeSpan.FromSeconds(3.0),
        TimeSpan.FromSeconds(5.0)
        };
        private readonly WebApplicationFactory<TStartup> _factory;
        public GrpcChannel Channel { get; }
        public HttpClient Client { get; }

        public GrpcTestFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _factory = new WebApplicationFactory<TStartup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddDbContext<LibraryDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryLibraryDb");
                        });
                    });
                   
                    builder.ConfigureKestrel(options =>
                    {
                        // Ensure Kestrel uses HTTP/2
                        options.ListenLocalhost(5001, o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
                    });
                });

            Client = _factory.CreateDefaultClient();
            Channel = GrpcChannel.ForAddress(Client.BaseAddress!, new GrpcChannelOptions
            {
                HttpClient = Client,
            });
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            dbContext.Database.EnsureCreated();  

           
            dbContext.Books.AddRange(
                new Book { Title = "1984", Author = "George Orwell", TotalPages = 328, TotalCopies = 5, ISBN = "978-93-5300-895-6" },
                new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", TotalPages = 281, TotalCopies = 3, ISBN = "978-45-8957-700-6" }
            );
            dbContext.Users.AddRange(
                new User { Name = "John Doe", Email = "john.doe@example.com" },
                new User { Name = "Jane Smith", Email = "jane.smith@example.com" }
            );
            dbContext.BorrowRecords.AddRange(
                new BorrowRecord { BookId = 1, UserId = 1, BorrowedDate = DateTime.Now.AddDays(-10), ReturnedDate = DateTime.Now.AddDays(-5) },
                new BorrowRecord { BookId = 2, UserId = 2, BorrowedDate = DateTime.Now.AddDays(-8), ReturnedDate = DateTime.Now.AddDays(-3) }
            );

            dbContext.SaveChanges();  
        }
        protected TResponse Execute<TResponse>(Func<TResponse> action)
        {
            return Policy.Handle<RpcException>(ShouldBeRetried)
                         .WaitAndRetry(RetrySpans, (exception, timeSpan) => { })
                         .Execute(action);
        }

        protected async Task<TResponse> ExecuteAsync<TResponse>(Func<Task<TResponse>> action)
        {
            return await Policy.Handle<RpcException>(ShouldBeRetried)
                               .WaitAndRetryAsync(RetrySpans, (exception, timeSpan) => { /* Log retry here if needed */ })
                               .ExecuteAsync(action);
        }

        private static bool ShouldBeRetried(RpcException exception)
        {
            return exception.Status.Detail.Equals(EnvoyProxyErrorMessage, StringComparison.InvariantCultureIgnoreCase);
        }

        public void Dispose()
        {
            Channel?.Dispose();
            Client?.Dispose();
            _factory?.Dispose();
        }
    }
}
