using MuscleGain.Core.Models.Statistics;

namespace MuscleGain.Core.Contracts
{
    public interface IStatisticsService
    {
        Task<StatisticsServiceModel> Total();
    }
}
