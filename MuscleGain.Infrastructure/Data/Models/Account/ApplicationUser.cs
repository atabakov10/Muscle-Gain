
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MuscleGain.Infrastructure.Data.Models.Account
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(DataConstants.FirstNameMaxLength)]
        public string? FirstName { get; set; }
        [StringLength(DataConstants.LastNameMaxLength)]
        public string? LastName { get; set; }
    }
}
