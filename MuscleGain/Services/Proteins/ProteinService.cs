using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Models.Proteins;
using static MuscleGain.Services.Proteins.ProteinQueryServiceModel;


namespace MuscleGain.Services.Proteins
{
    public class ProteinService : IProteinService
    {
        private readonly MuscleGainDbContext data;

        public ProteinService(MuscleGainDbContext data)
        => this.data = data;
            
        
        public async Task<ProteinQueryServiceModel> All(
            string flavour,
            string searchTerm,
            ProteinSorting sorting,
            int currentPage,
            int proteinsPerPage)
        {
            var proteinsQuery = this.data.Proteins.AsQueryable();

            if (!string.IsNullOrWhiteSpace(flavour))
            {
                proteinsQuery = proteinsQuery.Where(p => p.Flavour == flavour);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                proteinsQuery = proteinsQuery.Where(p => (p.Name + " " + p.Flavour).ToLower().Contains(searchTerm.ToLower())
                                                         || p.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            proteinsQuery = sorting switch
            {
                ProteinSorting.DateCreated => proteinsQuery.OrderByDescending(p => p.Id),
                ProteinSorting.NameAndFlavour => proteinsQuery.OrderBy(p => p.Name).ThenBy(p => p.Flavour),
                _ => proteinsQuery.OrderByDescending(p => p.Id)
            };

            var totalProteins = await proteinsQuery.CountAsync();

            var proteins = await proteinsQuery
                .Skip((currentPage - 1) * proteinsPerPage)
                .Take(proteinsPerPage)
                .Select(p => new ProteinServiceModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Flavour = p.Flavour,
                    Grams = p.Grams,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Category = p.ProteinCategory.Name
                })
                .ToListAsync();


            return new ProteinQueryServiceModel
            {
                TotalProteins = totalProteins,
                CurrentPage = currentPage,
                ProteinsPerPage = proteinsPerPage,
                Proteins = proteins
            };
        }

        public async Task<IEnumerable<string>> AllProteinFlavours()
        => await this.data
                .Proteins
                .Select(p => p.Flavour)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();
        
    }
}
