﻿using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Statistics;
using MuscleGain.Infrastructure.Data;

namespace MuscleGain.Core.Services.Statistics
{
	public class StatisticsService : IStatisticsService
	{
		private readonly MuscleGainDbContext dbContext;

		public StatisticsService(MuscleGainDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<StatisticsServiceModel> Total()
		{
			var totalProteins = await this.dbContext.Proteins.Where(x => x.IsDeleted == false && x.IsApproved == true && x.OrderId == null).CountAsync();
			var totalOrders = await this.dbContext.Orders.Where(x=> x.OrderStatus == "approved").CountAsync();

			return new StatisticsServiceModel
			{
				TotalProteins = totalProteins,
				TotalOrders = totalOrders
			};
		}
	}
}
