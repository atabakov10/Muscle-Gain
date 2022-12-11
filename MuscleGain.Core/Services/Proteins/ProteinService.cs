using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Home;
using MuscleGain.Core.Models.Proteins;
using MuscleGain.Core.Models.Reviews;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Protein;



namespace MuscleGain.Core.Services.Proteins
{
	public class ProteinService : IProteinService
	{
		private readonly MuscleGainDbContext dbContext;
		private readonly ICategoryService categoryService;
		private readonly ICloudinaryService cloudinaryService;
		private readonly IReviewService reviewService;

		private readonly string defaultArticlePicture = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg";

		public ProteinService(
			MuscleGainDbContext dbContext,
			ICategoryService categoryService,
			IReviewService reviewService,
			ICloudinaryService cloudinaryService)
		{
			this.dbContext = dbContext;
			this.categoryService = categoryService;
			this.reviewService = reviewService;
			this.cloudinaryService = cloudinaryService;
		}

		public async Task<ProteinQueryServiceModel> AllAsync(
			string? category = null,
			string? searchTerm = null,
			ProteinSorting sorting = ProteinSorting.DateCreated,
			int currentPage = 1,
			int proteinsPerPage = 1)
		{
			var proteinsQuery = dbContext.Proteins
				.Include(c => c.Reviews)
				.Where(x => x.IsDeleted == false && x.IsApproved == true && x.OrderId == null);

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
				ProteinSorting.Price => proteinsQuery.OrderBy(p => p.Price),
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

		public async Task Delete(int proteinId)
		{
			var proteinById = await this.GetProteinById(proteinId);
			if (proteinById == null)
			{
				throw new Exception("Invalid Id");
			}

			proteinById.IsDeleted = true;
			await this.dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<ProteinListingViewModel>> GetAllNotApproved()
		{
			var allProteins = await this.dbContext.Proteins
				.Where(c => c.IsDeleted == false && c.IsApproved == false)
				.Select(c => new ProteinListingViewModel()
				{
					Id = c.Id,
					Name = c.Name,
					ImageUrl = c.ImageUrl,
					Price = c.Price,
				}).ToListAsync();

			return allProteins;
		}

		public async Task ApproveProtein(int proteinId)
		{
			var protein = await this.GetProteinById(proteinId);
			if (protein == null)
			{
				throw new Exception("Id does not exist.");
			}

			protein.IsApproved = true;
			await this.dbContext.SaveChangesAsync();
		}

		public async Task UnapproveProtein(int proteinId)
		{
			var protein = await this.GetProteinById(proteinId);

			if (protein == null)
			{
				throw new Exception("Id does not exist.");
			}

			protein.IsDeleted = true;
			await this.dbContext.SaveChangesAsync();
		}

		public async Task AddAsync(AddProtein protein, IFormFile? file)
		{
			string imageUrl = this.defaultArticlePicture;

			if (file != null)
			{
				imageUrl = await this.cloudinaryService.UploudAsync(file);
			}

			var product = new Protein()
			{
				Name = protein.Name,
				Grams = protein.Grams,
				Flavour = protein.Flavour,
				Price = (decimal)protein.Price,
				Description = protein.Description,
				ImageUrl = imageUrl,
				CategoryId = protein.CategoryId,
				ApplicationUserId = protein.UserId
			};

			await this.dbContext.AddAsync(product);
			await this.dbContext.SaveChangesAsync();

		}


		public async Task<EditProteinViewModel> GetForEditAsync(int id)
		{
			var protein = await dbContext.FindAsync<Protein>(id);
			var picture = await dbContext.Proteins.Where(x => x.ImageUrl == protein.ImageUrl).FirstOrDefaultAsync();

			if (id == null)
			{
				throw new Exception();
			}

			if (protein.IsDeleted != false || protein.IsApproved != true || protein.OrderId != null)
			{
				throw new Exception();
			}

			var model = new EditProteinViewModel
			{
				Id = id,
				Name = protein.Name,
				Flavour = protein.Flavour,
				Grams = protein.Grams,
				Price = protein.Price,
				Description = protein.Description,
				ImageUrl =protein.ImageUrl,
				CategoryId = protein.CategoryId,
				Categories = await categoryService.GetAllCategories(),
				UserId = protein.ApplicationUserId
			};

			return model;
		}

		public async Task EditAsync(EditProteinViewModel model, IFormFile? file)
		{
			var entity = await dbContext.FindAsync<Protein>(model.Id);

			string imageUrl = entity.ImageUrl;

			if (file != null)
			{
				imageUrl = await this.cloudinaryService.UploudAsync(file);
			}

			entity.Name = model.Name;
			entity.Flavour = model.Flavour;
			entity.Grams = model.Grams;
			entity.Price = (decimal)model.Price;
			entity.Description = model.Description;
			entity.ImageUrl = imageUrl;
			entity.CategoryId = model.CategoryId;
			entity.ApplicationUserId = model.UserId;

			await this.dbContext.SaveChangesAsync();
		}

		public async Task<ProteinDetailsViewModel> GetForDetailsAsync(int id)
		{
			var protein = await this.dbContext
				.Proteins
				.Where(x=> x.IsDeleted == false && x.OrderId == null)
				.Include(r => r.Reviews)
				.ThenInclude(u => u.User)
				.Include(x => x.ProteinCategory)
				.Include(x => x.ApplicationUser)
				.FirstOrDefaultAsync(x => x.Id == id);


			if (id == null)
			{
				throw new ArgumentException("Id does not exist.");
			}

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
				CreatorFullName = $"{protein.ApplicationUser.FirstName} {protein.ApplicationUser.LastName}",
				Email = protein.ApplicationUser.Email,
				AvgRating = await reviewService.GetAverageRating(id) ,
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
			return await dbContext.Proteins
				.Where(x => x.IsDeleted == false && x.IsApproved == true && x.OrderId == null)
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

		public async Task<Protein?> GetProteinById(int id)
			=> await this.dbContext.FindAsync<Protein>(id);
	}
}
