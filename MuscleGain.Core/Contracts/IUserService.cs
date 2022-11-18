using MuscleGain.Core.Models.Users;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Core.Contracts
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
