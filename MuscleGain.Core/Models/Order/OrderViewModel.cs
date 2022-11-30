using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuscleGain.Core.Models.Cart;

namespace MuscleGain.Core.Models.Order
{
	public class OrderViewModel
	{
		public int Id { get; set; }

		public DateTime OrderDate { get; set; }

		public decimal TotalPrice { get; set; }

		public string PaymentStatus { get; set; } = null!;

		public string OrderStatus { get; set; } = null!;

		public string TransactionId { get; set; } = null!;


		public ICollection<ShoppingCartProteinViewModel> Proteins = new List<ShoppingCartProteinViewModel>();
	}
}
