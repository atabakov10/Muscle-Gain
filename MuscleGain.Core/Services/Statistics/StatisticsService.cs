using MuscleGain.Core.Contracts;
using MuscleGain.Infrastructure.Data;

namespace MuscleGain.Core.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly MuscleGainDbContext data;

        public StatisticsService(MuscleGainDbContext data)
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalProteins = this.data.Proteins.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalProteins = totalProteins,
                TotalUsers = totalUsers
            };
        }
    }
}
