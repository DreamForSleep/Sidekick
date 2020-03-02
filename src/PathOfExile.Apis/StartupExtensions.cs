using Microsoft.Extensions.DependencyInjection;
using PathOfExile.Apis.Official.Trade.Leagues;

namespace PathOfExile.Apis
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddPathOfExileServices(this IServiceCollection services)
        {
            // Http Services
            services.AddHttpClient();

            services.AddSingleton<ILeagueApi, LeagueApi>();

            return services;
        }
    }
}
