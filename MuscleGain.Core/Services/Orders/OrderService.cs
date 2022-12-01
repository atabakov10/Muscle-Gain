using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Cart;
using MuscleGain.Core.Models.Order;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Core.Services.Orders
{
	public class OrderService : IOrderService
	{
		private readonly IRepository repo;

		public OrderService(IRepository repo)
		{
			this.repo = repo;
		}

		public async Task<int> AddOrder(OrderViewModel model, string userId)
		{
			var user = await this.repo.GetByIdAsync<ApplicationUser>(userId);

			var proteins = new List<Protein>();

			foreach (var protein in model.Proteins)
			{
				proteins.Add(await this.repo.GetByIdAsync<Protein>(protein.Id));
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

			await this.repo.AddAsync(order);
			await this.repo.SaveChangesAsync();

			return order.Id;
		}

		public async Task<OrderViewModel> GetOrderById(int orderId)
		{
			var order = await this.repo.All<Order>().Where(o => o.Id == orderId).Include(o => o.Proteins).Select(o => new OrderViewModel()
			{
				Id = o.Id,
				OrderDate = o.OrderDate,
				TotalPrice = o.OrderTotal,
				PaymentStatus = o.PaymentStatus,
				OrderStatus = o.OrderStatus,
				Proteins = o.Proteins.Select(c => new ShoppingCartProteinViewModel()
				{
					Id = c.Id,
					ImageUrl = c.ImageUrl,
					Name = c.Name,
					Price = c.Price,
					CreatorFullName = $"{c.ApplicationUser.FirstName} {c.ApplicationUser.LastName}",
				}).ToList(),
			}).FirstOrDefaultAsync();

			return order;
		}

		public Task<IEnumerable<Order>> GetUserOrders(string name)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Order>> OrderProductsByOrderId(int id)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateOrder(OrderViewModel model)
		{
			var order = await this.repo.GetByIdAsync<Order>(model.Id);

			order.PaymentStatus = model.PaymentStatus;
			order.OrderStatus = model.OrderStatus;
			order.OrderDate = model.OrderDate;
			order.TransactionId = model.TransactionId;
			order.OrderTotal = model.TotalPrice;

			this.repo.Update(order);
			await this.repo.SaveChangesAsync();
		}
	}
}
