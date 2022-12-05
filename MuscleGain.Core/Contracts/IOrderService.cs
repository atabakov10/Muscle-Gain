using MuscleGain.Core.Models.Order;
using MuscleGain.Infrastructure.Data.Models.Protein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Core.Contracts
{
	public interface IOrderService
	{
		public Task<int> AddOrder(OrderViewModel model, string userId);

		public Task UpdateOrder(OrderViewModel model);
	}
}
