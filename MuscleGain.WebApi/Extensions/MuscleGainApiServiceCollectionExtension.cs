using System;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Services.Statistics;
using MuscleGain.Infrastructure.Data;

namespace MuscleGain.WebApi.Extensions
{
	public static class MuscleGainApiServiceCollectionExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IStatisticsService, StatisticsService>();

			return services;
		}

		public static IServiceCollection AddMuscleGainDbContext(this IServiceCollection services, IConfiguration config)
		{
			var connectionString = config.GetConnectionString("DefaultConnection");
			services.AddDbContext<MuscleGainDbContext>(options =>
				options.UseSqlServer(connectionString));

			return services;
		}
	}
}
