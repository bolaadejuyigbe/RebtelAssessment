using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.Extensions.Options;
using static Grpc.Core.Interceptors.Interceptor;

namespace Library.Api.Interceptors
{
    public class ClientDeadlineInterceptor : Interceptor
    {
        private readonly GrpcConfiguration _grpcConfiguration;
        private readonly ILogger<ClientDeadlineInterceptor> _logger;

        public ClientDeadlineInterceptor(IOptions<GrpcConfiguration> grpcConfiguration, ILogger<ClientDeadlineInterceptor> logger)
        {
            _grpcConfiguration = grpcConfiguration.Value ?? throw new ArgumentNullException(nameof(grpcConfiguration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var options = context.Options;
            if (context.Options.Deadline == null)
            {
                var deadline = DateTime.UtcNow.AddMilliseconds(_grpcConfiguration.DeadlineInMilliseconds);
                _logger.LogWarning("Client deadline was not set. Forcing deadline to {Milliseconds} ms from now.", _grpcConfiguration.DeadlineInMilliseconds);
                options = context.Options.WithDeadline(deadline);
            }
            var clientInterceptorContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
            return continuation(request, clientInterceptorContext);
        }
    }

    public class GrpcConfiguration
    {
        public int DeadlineInMilliseconds { get; set; } = 30000; // Default to 30000 milliseconds
    }
}
