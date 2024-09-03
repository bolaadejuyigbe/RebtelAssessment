using Library.DataModel.Database;
using Library.Grpc.Health;
using Library.Grpc.Interceptors;
using Library.Grpc.Services;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace Library.Grpc
{
    public class StartupExtension
    {
        public StartupExtension(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<LoggingAndErrorHandlingInterceptor>();
            });

            services.Configure<DbSettings>(options =>
            {
                options.DbConnectionString = Configuration.GetValue<string>("DbConnectionString");
            });

            services.AddDbContext<LibraryDbContext>((serviceProvider, options) =>
            {
                var dbSettings = serviceProvider.GetRequiredService<IOptions<DbSettings>>().Value;
                options.UseSqlServer(dbSettings.DbConnectionString);
            });

            ModuleConfiguration.ConfigureServices(services);
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            // Migrate and seed the database
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
        }
    }
}
