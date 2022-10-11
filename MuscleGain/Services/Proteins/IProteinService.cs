using MuscleGain.Models.Proteins;

namespace MuscleGain.Services.Proteins
{
    public interface IProteinService
    {
       Task<ProteinQueryServiceModel> All(
            string flavour,
            string searchTerm,
            ProteinSorting sorting,
            int currentPage,
            int proteinsPerPage);

       Task<IEnumerable<string>> AllProteinFlavours();
    }
}
