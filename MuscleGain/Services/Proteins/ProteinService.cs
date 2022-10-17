using Microsoft.EntityFrameworkCore;
using MuscleGain.Contracts;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Models.Proteins;


namespace MuscleGain.Services.Proteins
{
    public class ProteinService : IProteinService
    {
        private readonly IConfiguration config;
        private readonly MuscleGainDbContext data;
        private readonly IRepository repo;

        public ProteinService(
            IConfiguration _config,
            MuscleGainDbContext _data,
            IRepository _repo)
        {
            config = _config;
            data = _data;
            repo = _repo;
        }


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

        public async Task Add(AddProtein protein)
        {
            var product = new Protein()
            {
                Name = protein.Name,
                Grams = protein.Grams,
                Flavour = protein.Flavour,
                Price = protein.Price,
                Description = protein.Description,
                ImageUrl = protein.ImageUrl,
                CategoryId = protein.CategoryId
            };

            await repo.AddAsync(product);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> AllProteinFlavours()
        => await this.data
                .Proteins
                .Select(p => p.Flavour)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();

        public async Task Delete(int id)
        {
            var product = await repo.All<Protein>()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                product.IsActive = false;

                await repo.SaveChangesAsync();
            }
        }
    }
}
