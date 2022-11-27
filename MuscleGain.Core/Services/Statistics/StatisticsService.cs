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
        private readonly IRepository repo;

        public StatisticsService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<StatisticsServiceModel> Total()
        {
            var totalProteins = await this.repo.AllReadonly<Protein>().Where(x=> x.IsDeleted == false).CountAsync();
            var totalUsers = await this.repo.AllReadonly<ApplicationUser>().CountAsync();

            return new StatisticsServiceModel
            {
                TotalProteins = totalProteins,
                TotalUsers = totalUsers
            };
        }
    }
}
