using MuscleGain.Core.Models.Order;

namespace MuscleGain.Core.Contracts
{
	public interface IOrderService
	{
		public Task<int> AddOrder(OrderViewModel model, string userId);

		public Task UpdateOrder(OrderViewModel model);
	}
}
