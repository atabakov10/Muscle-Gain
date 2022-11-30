using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Core.Services.Payments
{
	public class StripeSettings
	{
		public string SecretKey { get; set; } = null!;

		public string PublishableKey { get; set; } = null!;
	}
}
