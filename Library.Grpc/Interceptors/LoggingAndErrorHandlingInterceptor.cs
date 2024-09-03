using Grpc.Core.Interceptors;
using Grpc.Core;
using System.Diagnostics;

namespace Library.Grpc.Interceptors
{
    public class LoggingAndErrorHandlingInterceptor : Interceptor
    {
        private readonly ILogger<LoggingAndErrorHandlingInterceptor> _logger;

        public LoggingAndErrorHandlingInterceptor(ILogger<LoggingAndErrorHandlingInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var serviceAndMethod = context.Method.TrimStart('/').Split('/');
            var service = serviceAndMethod.First();
            var method = serviceAndMethod.Last();

            if (service == "grpc.health.v1.Health")
            {
                return await continuation(request, context);
            }

            var success = false;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation("Received request for {Service}.{Method} with data: {@Request}", service, method, request);

                var response = await continuation(request, context);

                _logger.LogInformation("Sending response for {Service}.{Method} with data: {@Response}, Duration: {ElapsedMilliseconds}ms", service, method, response, stopwatch.ElapsedMilliseconds);

                success = true;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling gRPC request for {Service}.{Method}, Duration: {ElapsedMilliseconds}ms", service, method, stopwatch.ElapsedMilliseconds);

                var rpcException = CreateRpcException(ex);
                throw rpcException;
            }
            finally
            {
                stopwatch.Stop();
                LogRequestMetrics(service, method, success, stopwatch.ElapsedMilliseconds);
            }
        }

        private static RpcException CreateRpcException(Exception ex)
        {
            var trailers = new Metadata();
            trailers.Add("correlation-id", Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString());

            return new RpcException(new Status(StatusCode.Internal, "An error occurred while processing your request."), trailers);
        }

        private void LogRequestMetrics(string service, string method, bool success, long elapsedMilliseconds)
        {
            _logger.LogInformation("Request metrics - Service: {Service}, Method: {Method}, Success: {Success}, Duration: {ElapsedMilliseconds}ms",
                service, method, success, elapsedMilliseconds);
        }
    }
}