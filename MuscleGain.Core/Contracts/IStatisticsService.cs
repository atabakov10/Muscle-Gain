using MuscleGain.Core.Services.Statistics;

namespace MuscleGain.Core.Contracts
{
    public interface IStatisticsService
    {
        Task<StatisticsServiceModel> Total();
    }
}
