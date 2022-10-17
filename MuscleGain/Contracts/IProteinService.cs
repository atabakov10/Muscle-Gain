using MuscleGain.Models.Proteins;
using MuscleGain.Services.Proteins;

namespace MuscleGain.Contracts
{
    public interface IProteinService
    {
        Task<ProteinQueryServiceModel> All(
             string flavour,
             string searchTerm,
             ProteinSorting sorting,
             int currentPage,
             int proteinsPerPage);

        Task Add(AddProtein protein);

        Task<IEnumerable<string>> AllProteinFlavours();

        Task Delete(int id);
    }
}
