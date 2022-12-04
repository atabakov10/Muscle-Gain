using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Statistics;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;

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
            var totalProteins = await this.dbContext.Proteins.Where(x=> x.IsDeleted == false).CountAsync();
            var totalUsers = await this.dbContext.Users.CountAsync();

            return new StatisticsServiceModel
            {
                TotalProteins = totalProteins,
                TotalUsers = totalUsers
            };
        }
    }
}
