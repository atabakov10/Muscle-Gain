using MuscleGain.Core.Models.Home;
using MuscleGain.Core.Models.Proteins;
using MuscleGain.Core.Services.Proteins;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Core.Contracts
{
    public interface IProteinService
    {
		Task<ProteinQueryServiceModel> AllAsync(
			 string? flavour = null,
             string? searchTerm = null,
             ProteinSorting sorting = ProteinSorting.DateCreated,
             int currentPage = 1,
             int proteinsPerPage = 1);

        Task AddAsync(AddProtein protein);

        Task<EditProteinViewModel> GetForEditAsync(int id);

        Task EditAsync(EditProteinViewModel model);

        Task<ProteinDetailsViewModel> GetForDetailsAsync(int id);

        Task<IEnumerable<ProteinIndexViewModel>> LastThreeProteins();

        Task<Protein?> GetProteinById(int id);

        Task<IEnumerable<ProteinListingViewModel>> GetAllNotApproved();

        Task ApproveProtein(int proteinId);

        Task UnapproveProtein(int proteinId);
        Task Delete(int proteinId);

    }
}
