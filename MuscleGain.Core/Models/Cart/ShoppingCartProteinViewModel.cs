using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Core.Models.Cart
{
	public class ShoppingCartProteinViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string ImageUrl { get; set; }

		public decimal Price { get; set; }
	}
}
