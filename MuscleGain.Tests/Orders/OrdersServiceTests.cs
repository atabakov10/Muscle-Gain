using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Models.Order;
using MuscleGain.Core.Services.Orders;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Tests.Orders
{
	[TestFixture]
	public class OrdersServiceTests
	{
		[Test]
		public async Task UpdateOrderShouldUpdateTheOrder()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var orderService = new OrderService(dbContext);
			var user = new ApplicationUser()
			{
				Id = "tabaka10",
				FirstName = "Angel",
				LastName = "Tabakov",
			};
		
			var order = new Order()
			{
				Id = 1,
				OrderDate = DateTime.Today,
				OrderStatus = "approved",
				PaymentStatus = "approved",
				TransactionId = "fakeId",
				CustomerId = user.Id,
				OrderTotal = new decimal(40.00)
			};
			var orderViewModel = new OrderViewModel()
			{
				Id = order.Id,
				OrderDate = order.OrderDate,
				OrderStatus = order.OrderStatus,
				PaymentStatus = order.PaymentStatus,
				TransactionId = order.TransactionId,
				TotalPrice = new decimal(50.00)
			};

			await dbContext.AddAsync(order);
			await dbContext.SaveChangesAsync();

			await orderService.UpdateOrder(orderViewModel);

			Assert.NotNull(dbContext.Orders.Count());
			Assert.That(dbContext.Orders.FirstOrDefault().OrderTotal, Is.EqualTo(50.00));
			Assert.AreEqual("approved", dbContext.Orders.FirstOrDefault().OrderStatus);
		}

		[Test]
		public async Task GetAllOrdersShouldGetAllOrders()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var orderService = new OrderService(dbContext);

			var user = new ApplicationUser()
			{
				Id = "tabaka10",
				FirstName = "Angel",
				LastName = "Tabakov",
			};

			var order = new Order()
			{
				Id = 1,
				OrderDate = DateTime.Today,
				OrderStatus = "approved",
				PaymentStatus = "approved",
				TransactionId = "fakeId",
				CustomerId = user.Id,
				OrderTotal = new decimal(40.00)
			};

			var orderTwo = new Order()
			{
				Id = 2,
				OrderDate = DateTime.Today,
				OrderStatus = "approved",
				PaymentStatus = "approved",
				TransactionId = "fakeId",
				CustomerId = user.Id,
				OrderTotal = new decimal(50.00)
			};

			await dbContext.Users.AddAsync(user);
			await dbContext.Orders.AddAsync(order);
			await dbContext.Orders.AddAsync(orderTwo);
			await dbContext.SaveChangesAsync();

			var result = await orderService.GetAllOrders(order.CustomerId);

			Assert.That(result.Count, Is.EqualTo(2));
		}
	}
}
