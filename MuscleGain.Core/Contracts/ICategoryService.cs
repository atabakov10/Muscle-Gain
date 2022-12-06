using MuscleGain.Core.Models.Category;

namespace MuscleGain.Core.Contracts
{
    public interface ICategoryService
    {
        public Task<IEnumerable<ProteinCategoryViewModel>> GetAllCategories();

        public Task CreateCategory(ProteinCategoryViewModel model);

        Task<EditCategoryViewModel> GetCategoryForEdit(int id);

        Task Update(EditCategoryViewModel model);

        Task Delete(int id);

        Task<IEnumerable<string>> AllProteinCategoriesAsync();

		Task CheckForCategory(int categoryId);

    }
}
