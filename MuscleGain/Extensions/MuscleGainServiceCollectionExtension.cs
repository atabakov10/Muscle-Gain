using MuscleGain.Contracts;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Services.Proteins;
using MuscleGain.Services.Statistics;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MuscleGainServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();

            services.AddScoped<IStatisticsService, StatisticsService>();

            services.AddScoped<IProteinService, ProteinService>();


            return services;
        }
    }
}
