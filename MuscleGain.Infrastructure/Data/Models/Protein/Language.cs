using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Infrastructure.Data.Models.Protein
{
	public class Language
	{
		public int Id { get; set; }

		[Required]
		[StringLength(30)]
		public string Title { get; set; } = null!;

		public bool IsDeleted { get; set; }

		public ICollection<Protein> Proteins { get; set; } = new HashSet<Protein>();
	}
}
