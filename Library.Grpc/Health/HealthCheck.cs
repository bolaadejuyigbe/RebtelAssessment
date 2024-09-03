using Grpc.Health.V1;
using Grpc.HealthCheck;

namespace Library.Grpc.Health
{
    public class HealthCheck: HealthServiceImpl
    {
        public HealthCheck() 
        {
            SetStatus(string.Empty, HealthCheckResponse.Types.ServingStatus.Serving);
        }    
    }
}
