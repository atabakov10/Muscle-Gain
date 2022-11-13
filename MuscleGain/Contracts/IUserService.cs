using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Models.Users;

namespace MuscleGain.Contracts
{
	public interface IUserService
	{
        Task<IEnumerable<UserListViewModel>> GetUsers();

        Task<UserProfileViewModel> GetUserProfile(string id);

        Task<UserEditViewModel> GetUserForEdit(string id);

        Task<bool> UpdateUser(UserEditViewModel model);

        Task<ApplicationUser> GetUserById(string id);

    }
}
