using MuscleGain.Contracts;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Cart;
using MuscleGain.Services.Proteins;
using MuscleGain.Services.ShoppingCart;
using MuscleGain.Services.Statistics;
using MuscleGain.Services.User;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MuscleGainServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();

            services.AddScoped<IStatisticsService, StatisticsService>();

            services.AddScoped<IProteinService, ProteinService>();

            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
