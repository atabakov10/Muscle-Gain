using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Home;
using MuscleGain.Core.Models.Proteins;
using MuscleGain.Core.Models.Reviews;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Infrastructure.Data.Models.Reviews;



namespace MuscleGain.Core.Services.Proteins
{
    public class ProteinService : IProteinService
    {
        private readonly IRepository repo;

        public ProteinService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<ProteinQueryServiceModel> AllAsync(
            string? category = null,
            string? searchTerm = null,
            ProteinSorting sorting = ProteinSorting.DateCreated,
            int currentPage = 1,
            int proteinsPerPage = 1)
        {
            var proteinsQuery = repo.AllReadonly<Protein>();

            if (!string.IsNullOrEmpty(category))
            {
                proteinsQuery = proteinsQuery
                    .Where(p => p.ProteinCategory.Name == category);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
	            searchTerm = $"%{searchTerm.ToLower()}%";
                proteinsQuery = proteinsQuery
                    .Where(p => EF.Functions.Like(p.Name.ToLower(), searchTerm)
                                || EF.Functions.Like(p.Flavour.ToLower(), searchTerm)
                                || EF.Functions.Like(p.Description.ToLower(), searchTerm));
            }

            proteinsQuery = sorting switch
            {
                ProteinSorting.DateCreated => proteinsQuery.OrderByDescending(p => p.Id),
                ProteinSorting.NameAndFlavour => proteinsQuery.OrderBy(p => p.Name).ThenBy(p => p.Flavour),
                ProteinSorting.Price => proteinsQuery.OrderBy(p=> p.Price),
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

        public async Task AddAsync(AddProtein protein)
        {
            var product = new Protein()
            {
                Name = protein.Name,
                Grams = protein.Grams,
                Flavour = protein.Flavour,
                Price = (decimal)protein.Price,
                Description = protein.Description,
                ImageUrl = protein.ImageUrl,
                CategoryId = protein.CategoryId
            };

            await repo.AddAsync(product);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> AllProteinCategoriesAsync()
        => await repo.AllReadonly<ProteinsCategories>()
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();


        public async Task<IEnumerable<ProteinCategoryViewModel>> GetProteinCategoriesAsync()
        {
            return await repo.AllReadonly<ProteinsCategories>()
                .Select(x => new ProteinCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<EditProteinViewModel> GetForEditAsync(int id)
        {
            var protein = await repo.GetByIdAsync<Protein>(id);

            var model = new EditProteinViewModel()
            {
                Id = id,
                Name = protein.Name,
                Flavour = protein.Flavour,
                Grams = protein.Grams,
                Price = protein.Price,
                Description = protein.Description,
                ImageUrl = protein.ImageUrl,
                CategoryId = protein.CategoryId
            };

            model.Categories = await GetProteinCategoriesAsync();

            return model;
        }

        public async Task EditAsync(EditProteinViewModel model)
        {
            var entity = await repo.GetByIdAsync<Protein>(model.Id);

            entity.Name = model.Name;
            entity.Flavour = model.Flavour;
            entity.Grams = model.Grams;
            entity.Price = (decimal)model.Price;
            entity.Description = model.Description;
            entity.ImageUrl = model.ImageUrl;
            entity.CategoryId = model.CategoryId;

            await repo.SaveChangesAsync();
        }

        public async Task<ProteinDetailsViewModel> GetForDetailsAsync(int id)
        {
            var protein = await this.repo
                .AllReadonly<Protein>()
                .Include(r => r.Reviews)
                .ThenInclude(u => u.User)
                .Include(x => x.ProteinCategory)
                .FirstOrDefaultAsync(x => x.Id == id);



            if (protein == null)
            {
                throw new ArgumentException("Invalid protein Id");
            }


            return new ProteinDetailsViewModel()
            {
                Id = id,
                Name = protein.Name,
                Flavour = protein.Flavour,
                Grams = protein.Grams,
                Price = protein.Price,
                Description = protein.Description,
                ImageUrl = protein.ImageUrl,
                Category = protein.ProteinCategory.Name,

                Reviews = protein.Reviews.Select(r => new ReviewViewModel()
                {
                    UserFullName = $"{r.User.FirstName} {r.User.LastName}",
                    Comment = r.Comment,
                    Rating = r.Rating,
                    ImageUrl = r.User.ImageUrl,
                    DateOfPublication = r.DateOfPublication.ToString()
                }).ToList(),
            };
        }

        public async Task<IEnumerable<ProteinIndexViewModel>> LastThreeProteins()
        {
            return await repo.AllReadonly<Protein>()
                .OrderByDescending(p => p.Id)
                .Select(p => new ProteinIndexViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Grams = p.Grams,
                    Flavour = p.Flavour,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                })
                .Take(3)
                .ToListAsync();
        }
        public async Task<Protein> GetProteinById(int id)
        {
	        return await this.repo.AllReadonly<Protein>().Include(p => p.ProteinCategory)
		        .Include(x => x.ImageUrl)
		        .Include(x => x.Reviews)
		        .FirstOrDefaultAsync(x => x.Id == id);
        }
	}
}
