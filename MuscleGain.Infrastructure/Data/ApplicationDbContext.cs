using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data.Models;

namespace MuscleGain.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MuscleGain.Infrastructure.Data.Models.Protein> Proteins { get; set; }
    }
}