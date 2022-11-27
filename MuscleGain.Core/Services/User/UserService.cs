using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Users;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Core.Services.User
{
	public class UserService : IUserService
	{
        private readonly IRepository repo;
        private readonly UserManager<ApplicationUser> userManager;
		public UserService(IRepository repo,
			UserManager<ApplicationUser> userManager)
		{
            this.repo = repo;
			this.userManager = userManager;
		}

		public async Task<ApplicationUser> GetUserById(string id) 
			=> await this.repo.GetByIdAsync<ApplicationUser>(id);
		

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
				Email = user.Email,
				PhoneNumber = user.PhoneNumber
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
				LastName = user.LastName,
				PhoneNumber = user.PhoneNumber
			};
		}

		public async Task<IEnumerable<UserListViewModel>> GetUsers()
		{
			var users = await this.repo.All<ApplicationUser>()
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


			if (user != null)
			{
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.ImageUrl = model.ImageUrl;
				user.PhoneNumber = model.PhoneNumber;
				
				await this.repo.SaveChangesAsync();
				result = true;
			}

			return result;
		}

		public async Task<ApplicationUser> GetUserByUsername(string username)
		{
			return await this.userManager.FindByNameAsync(username);
		}

	}
}
