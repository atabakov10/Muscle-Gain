using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Users;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Core.Services.User
{
	public class UserService : IUserService
	{
		private readonly MuscleGainDbContext data;

		public UserService(MuscleGainDbContext data)
		{
			this.data = data;
		}

		public async Task<ApplicationUser> GetUserById(string id)
		{
			return await this.data.Users.FirstOrDefaultAsync(context => context.Id == id);
		}

		public async Task<UserProfileViewModel> GetUserProfile(string id)
		{
			var user = await GetUserById(id);
			return new UserProfileViewModel()
			{
				Id = user.Id,
				ImageUrl = user.ImageUrl,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserName = user.UserName,
				Email = user.Email
			};
		}

		public async Task<UserEditViewModel> GetUserForEdit(string id)
		{
			var user = await GetUserById(id);

			return new UserEditViewModel()
			{
				Id = user.Id,
				ImageUrl = user.ImageUrl,
				FirstName = user.FirstName,
				LastName = user.LastName
			};
		}

		public async Task<IEnumerable<UserListViewModel>> GetUsers()
		{
			var users = await this.data.Users
				.Select(u => new UserListViewModel()
				{
					Email = u.Email,
					Id = u.Id,
					ImageUrl = u.ImageUrl,
					FullName = $"{u.FirstName} {u.LastName}",
					Username = u.UserName
				})
				.ToListAsync();

			return users;
		}

		public async Task<bool> UpdateUser(UserEditViewModel model)
		{
			bool result = false;
			var user = await GetUserById(model.Id);


			if (user != null)
			{
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.ImageUrl = model.ImageUrl;

				await this.data.SaveChangesAsync();
				result = true;
			}

			return result;
		}


	}
}
