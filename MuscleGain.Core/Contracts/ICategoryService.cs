using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
