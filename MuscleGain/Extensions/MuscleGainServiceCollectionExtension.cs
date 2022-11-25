﻿using MuscleGain.Core.Contracts;
using MuscleGain.Core.Services.Cart;
using MuscleGain.Core.Services.Language;
using MuscleGain.Core.Services.Proteins;
using MuscleGain.Core.Services.Review;
using MuscleGain.Core.Services.Statistics;
using MuscleGain.Core.Services.User;
using MuscleGain.Infrastructure.Data.Common;

namespace MuscleGain.Extensions
{
    public static class MuscleGainServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();

            services.AddScoped<IStatisticsService, StatisticsService>();

            services.AddScoped<IProteinService, ProteinService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ILanguageService, LanguageService>();

            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            services.AddScoped<IReviewService, ReviewService>();

            return services;
        }
    }
}
