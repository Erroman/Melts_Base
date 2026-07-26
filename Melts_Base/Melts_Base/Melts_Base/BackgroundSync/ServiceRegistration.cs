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
            services.AddSingleton<ApplicationPollingRuntimeOptions>();
            services.AddSingleton<IPollingRuntimeOptions>(provider => provider.GetRequiredService<ApplicationPollingRuntimeOptions>());
            services.AddSingleton<RealSybaseMeltSource>();
            services.AddSingleton<RealOracleMeltSource>();
            services.AddSingleton<SqliteFakeSybaseMeltSource>();
            services.AddSingleton<SqliteFakeOracleMeltSource>();
            services.AddSingleton<ISybaseMeltSource, RuntimeSybaseMeltSource>();
            services.AddSingleton<IOracleMeltSource, RuntimeOracleMeltSource>();
            services.AddSingleton<MeltPollingBackgroundService>();
            services.AddHostedService(provider => provider.GetRequiredService<MeltPollingBackgroundService>());
            return services;
        }
    }
}
