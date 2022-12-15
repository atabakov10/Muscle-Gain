using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Users;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Core.Services.User
{
	public class UserService : IUserService
	{
        private readonly MuscleGainDbContext dbContext;
        private readonly ICloudinaryService cloudinaryService;
       
		public UserService(MuscleGainDbContext dbContext,
			ICloudinaryService cloudinaryService)
		{
            this.dbContext = dbContext;
			this.cloudinaryService = cloudinaryService;
		
		}

        public async Task<ApplicationUser> GetUserById(string id)
        {
            if (id == null)
            {
                throw new NullReferenceException();
            }

            return await this.dbContext.FindAsync<ApplicationUser>(id);
        }

        public async Task<UserProfileViewModel> GetUserProfile(string id)
		{
			if (id == null)
			{
				throw new NullReferenceException("Id is invalid");
			}

			var user = await GetUserById(id);
		
			return new UserProfileViewModel()
			{
				Id = user.Id,
				ImageUrl = user.ImageUrl,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserName = user.UserName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber
			};
		}

		public async Task<UserEditViewModel> GetUserForEdit(string id)
		{
			if (id == null)
			{
				throw new NullReferenceException("Invalid Id");
			}

			var user = await GetUserById(id);

			return new UserEditViewModel()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				PhoneNumber = user.PhoneNumber
			};
		}

		public async Task<IEnumerable<UserListViewModel>> GetUsers()
		{
			var users = await this.dbContext.Users
				.Select(u => new UserListViewModel()
				{
					Email = u.Email,
					Id = u.Id,
					ImageUrl = u.ImageUrl,
					FullName = $"{u.FirstName} {u.LastName}",
					Username = u.UserName,
					PhoneNumber = u.PhoneNumber
				})
				.ToListAsync();

			return users;
		}

		public async Task<bool> UpdateUser(UserEditViewModel model)
		{
			bool result = false;
			var user = await GetUserById(model.Id);
			
			string imageUrl = user.ImageUrl;

			if (model.ImageUrl != null)
			{ 
				imageUrl = await this.cloudinaryService.UploudAsync(model.ImageUrl);
			}


			if (user != null)
			{
				user.Id = model.Id;
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.ImageUrl = imageUrl;
				user.PhoneNumber = model.PhoneNumber;
				
				await this.dbContext.SaveChangesAsync();
				result = true;
			}
			else
			{
				throw new NullReferenceException("Invalid user");
			}

			return result;
		}


	}
}
