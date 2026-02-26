using Melts_Base.SQLiteModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Melts_Base.BackgroundSync
{
    internal static class ServiceRegistration
    {
        public static IServiceCollection AddMeltBackgroundPolling(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PollingSettings>(configuration.GetSection("Polling"));
            services.AddDbContext<MeltContext>();

            var useFakeSources = configuration.GetValue<bool>("Polling:UseFakeSources");
            if (useFakeSources)
            {
                services.AddSingleton<ISybaseMeltSource, FakeSybaseMeltSource>();
                services.AddSingleton<IOracleMeltSource, FakeOracleMeltSource>();
            }
            else
            {
                services.AddSingleton<ISybaseMeltSource, RealSybaseMeltSource>();
                services.AddSingleton<IOracleMeltSource, RealOracleMeltSource>();
            }

            services.AddHostedService<MeltPollingBackgroundService>();
            return services;
        }
    }
}