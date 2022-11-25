using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MuscleGain.Infrastructure.Data.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Infrastructure.Data.Models.Cart
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Protein.Protein> ShoppingCartProteins { get; set; } = new HashSet<Protein.Protein>();

    }
}
