using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Polly;

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
            _factory = new WebApplicationFactory<TStartup>()
                .WithWebHostBuilder(builder =>
                {
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
