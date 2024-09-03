using Grpc.Core;
using Polly;

namespace LibraryService.Test
{
    public abstract class GrpcTestFixture
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
    }
}
