using Library.DataModel.Database;
using Library.Grpc.Interceptors;
using Library.Service.Persistence;
using Library.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Library.Grpc
{
    public static class ModuleConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Register your repositories
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            // Register your services
            services.AddScoped<ILibraryService, LibraryService>();

            services.AddSingleton<LoggingAndErrorHandlingInterceptor>();
        }
    }
}
