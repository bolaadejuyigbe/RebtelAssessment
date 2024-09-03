namespace Library.Grpc.Configuration
{
    public class KestrelConfiguration
    {
        public string Ip { get; set; } = "0.0.0.0"; // Default to all network interfaces
        public int GrpcPort { get; set; } = 5000; // Default gRPC port
        public int HttpPort { get; set; } = 80; 

        public KestrelConfiguration() { }

        public KestrelConfiguration(string ip, int grpcPort, int httpPort)
        {
            Ip = ip;
            GrpcPort = grpcPort;
            HttpPort = httpPort;
        }
    }
}
