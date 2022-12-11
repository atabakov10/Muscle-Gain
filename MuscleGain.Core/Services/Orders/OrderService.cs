using AngleSharp.Html;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Order;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Core.Services.Orders
{
	public class OrderService : IOrderService
	{
		private readonly MuscleGainDbContext dbContext;

		public OrderService(MuscleGainDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<int> AddOrder(OrderViewModel model, string userId)
		{
			var user = await this.dbContext.FindAsync<ApplicationUser>(userId);
			var proteins = new List<Protein>();

			foreach (var protein in model.Proteins)
			{
				proteins.Add(await this.dbContext.FindAsync<Protein>(protein.Id));
			}

			if (string.IsNullOrEmpty(model.TransactionId))
			{
				model.TransactionId = "none";
			}

			var order = new Order()
			{
				Customer = user,
				OrderStatus = model.OrderStatus,
				PaymentStatus = model.PaymentStatus,
				OrderTotal = model.TotalPrice,
				OrderDate = model.OrderDate,
				TransactionId = model.TransactionId,
				Proteins = proteins,
			};

			await this.dbContext.AddAsync(order);
			await this.dbContext.SaveChangesAsync();

			return order.Id;
		}


		public async Task UpdateOrder(OrderViewModel model)
		{
			var order = await this.dbContext.FindAsync<Order>(model.Id);

			order.PaymentStatus = model.PaymentStatus;
			order.OrderStatus = model.OrderStatus;
			order.OrderDate = model.OrderDate;
			order.TransactionId = model.TransactionId;
			order.OrderTotal = model.TotalPrice;

			this.dbContext.Update(order);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task<ICollection<Order>> GetAllOrders(string userId)
		{
			var ordersUser = await dbContext.Orders.Where(x => x.Customer.Id == userId).Include(x=> x.Customer)
				.Include(x=> x.Proteins)
				.ThenInclude(x=> x.ProteinCategory)
				.ToListAsync();
			return ordersUser;
		}
	}
}
