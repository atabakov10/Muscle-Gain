using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models;

namespace MuscleGain.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDataBase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope(); 

           var data = scopedServices.ServiceProvider.GetService<MuscleGainDbContext>();

            data.Database.Migrate();

            SeedProteinCategories(data);
             
            return app;
        }
        private static async void SeedProteinCategories(MuscleGainDbContext data)
        {
            if (data.ProteinsCategories.Any())
            {
                return;
            }
            data.ProteinsCategories.AddRange(new[]
            {
                new ProteinsCategories{Name = "Whey Protein"},
                new ProteinsCategories{Name = "Clear Protein"},
                new ProteinsCategories{Name = "Vegan Protein"},
                new ProteinsCategories{Name = "Milk & Casein"}
            });

           await data.SaveChangesAsync();
        }
    }
}
