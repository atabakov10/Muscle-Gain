using MuscleGain.Core.Contracts;
using MuscleGain.Core.Services.Cart;
using MuscleGain.Core.Services.Categories;
using MuscleGain.Core.Services.Orders;
using MuscleGain.Core.Services.Proteins;
using MuscleGain.Core.Services.Quote;
using MuscleGain.Core.Services.Review;
using MuscleGain.Core.Services.Statistics;
using MuscleGain.Core.Services.User;

namespace MuscleGain.Extensions
{
    public static class MuscleGainServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddScoped<IRepository, Repository>();

            services.AddScoped<IStatisticsService, StatisticsService>();

            services.AddScoped<IProteinService, ProteinService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IQuotesService, QuotesService>();

            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            services.AddScoped<IReviewService, ReviewService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
