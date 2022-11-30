using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MuscleGain.Infrastructure.Data.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Infrastructure.Data.Models.Protein
{
	public class Order
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string CustomerId { get; set; } = null!;
		public ApplicationUser Customer { get; set; } = null!;

		public DateTime OrderDate { get; set; }

		[Column(TypeName = "decimal(18, 2)")]
		public decimal OrderTotal { get; set; }

		[Required]
		[StringLength(50)]
		public string PaymentStatus { get; set; } = null!;

		[Required]
		[StringLength(50)]
		public string OrderStatus { get; set; } = null!;

		[Required]
		[StringLength(50)]
		public string TransactionId { get; set; } = null!;

		public ICollection<Protein> Proteins { get; set; } = new HashSet<Protein>();
	}
}
