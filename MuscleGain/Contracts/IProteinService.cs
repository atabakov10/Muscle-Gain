using MuscleGain.Models.Proteins;
using MuscleGain.Services.Proteins;

namespace MuscleGain.Contracts
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

        Task<IEnumerable<string>> AllProteinCategoriesAsync();

        Task<IEnumerable<ProteinCategoryViewModel>> GetProteinCategoriesAsync();

        Task<EditProteinViewModel> GetForEditAsync(int id);

        Task EditAsync(EditProteinViewModel model);

        Task<ProteinDetailsViewModel> GetForDetailsAsync(int id);
    }
}
