using Microsoft.EntityFrameworkCore;
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

//using MuscleGain.Infrastructure.Data.Common;


namespace MuscleGain.Core.Services.Proteins
{
    public class ProteinService : IProteinService
    {
        private readonly IRepository repo;
        private readonly IConfiguration config;
        private readonly MuscleGainDbContext data;

        public ProteinService(
            IConfiguration _config,
            MuscleGainDbContext _data,
            IRepository _repo)
        {
            config = _config;
            data = _data;
            repo = _repo;
        }

        public async Task<ProteinQueryServiceModel> AllAsync(
            string category,
            string searchTerm,
            ProteinSorting sorting,
            int currentPage,
            int proteinsPerPage)
        {
            var proteinsQuery = this.data.Proteins.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                proteinsQuery = proteinsQuery.Where(p => p.ProteinCategory.Name == category);
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

        public async Task AddAsync(AddProtein protein)
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

            await data.AddAsync(product);
            await data.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> AllProteinCategoriesAsync()
        => await this.data
                .Proteins
                .Select(p => p.ProteinCategory.Name)
                .Distinct()
                .OrderBy(n => n)
                .ToListAsync();


        public async Task<IEnumerable<ProteinCategoryViewModel>> GetProteinCategoriesAsync()
        {
            return await this.data
                .ProteinsCategories
                .Select(x => new ProteinCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<EditProteinViewModel> GetForEditAsync(int id)
        {
            var protein = await data.Proteins.FindAsync(id);

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
            var entity = await data.Proteins.FindAsync(model.Id);

            entity.Name = model.Name;
            entity.Flavour = model.Flavour;
            entity.Grams = model.Grams;
            entity.Price = model.Price;
            entity.Description = model.Description;
            entity.ImageUrl = model.ImageUrl;
            entity.CategoryId = model.CategoryId;

            await data.SaveChangesAsync();
        }

        public async Task<ProteinDetailsViewModel> GetForDetailsAsync(int id)
        {
	        var protein = await this.repo
		        .AllReadonly<Protein>()
		        .Include(r => r.Reviews)
		        .ThenInclude(u => u.User)
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

		        Reviews = protein.Reviews.Select(r => new ReviewViewModel()
		        {
			        UserFullName = $"{r.User.FirstName} {r.User.LastName}",
			        Comment = r.Comment,
			        Rating = r.Rating,
                    ImageUrl = r.User.ImageUrl,
			        DateOfPublication = r.DateOfPublication.ToString()
		        }).ToList(),

		        CategoryId = protein.CategoryId
	        };

        }
        public async Task AddReview(AddReviewViewModel model)
        {
	        var user = await this.repo.GetByIdAsync<ApplicationUser>(model.UserId);

	        if (user == null)
	        {
		        throw new Exception();
	        }

	        var course = await this.repo.GetByIdAsync<Protein>(model.ProteinId);

	        if (course == null)
	        {
		        throw new Exception();
	        }

	        var review = new Review()
	        {
		        Comment = model.Comment,
		        Rating = model.Rating,
		        ProteinId = model.ProteinId,
		        UserId = model.UserId,
		        DateOfPublication = model.DateOfPublication,
	        };

	        await this.repo.AddAsync(review);
	        await this.repo.SaveChangesAsync();
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
    }
}
