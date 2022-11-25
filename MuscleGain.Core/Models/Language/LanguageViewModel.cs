using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Core.Models.Language
{
	public class LanguageViewModel
	{ 
		public int Id { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 2)]
		public string Title { get; set; } = null!;
	}
}
