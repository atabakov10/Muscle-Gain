using MuscleGain.Core.Models.Home;
using MuscleGain.Core.Models.Proteins;
using MuscleGain.Core.Models.Reviews;
using MuscleGain.Core.Services.Proteins;

namespace MuscleGain.Core.Contracts
{
    public interface IProteinService
    {
        Task<ProteinQueryServiceModel> AllAsync(
             string flavour,
             string searchTerm,
             ProteinSorting sorting,
             int currentPage,
             int proteinsPerPage);

        Task AddAsync(AddProtein protein);

        Task AddReview(AddReviewViewModel model);

		Task<IEnumerable<string>> AllProteinCategoriesAsync();

        Task<IEnumerable<ProteinCategoryViewModel>> GetProteinCategoriesAsync();

        Task<EditProteinViewModel> GetForEditAsync(int id);

        Task EditAsync(EditProteinViewModel model);

        Task<ProteinDetailsViewModel> GetForDetailsAsync(int id);

        Task<IEnumerable<ProteinIndexViewModel>> LastThreeProteins();

    }
}
