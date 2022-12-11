using MuscleGain.Core.Models.Order;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Core.Contracts
{
	public interface IOrderService
	{
		public Task<int> AddOrder(OrderViewModel model, string userId);

		public Task UpdateOrder(OrderViewModel model);

		public Task<ICollection<Order>> GetAllOrders(string userId);
	}
}
