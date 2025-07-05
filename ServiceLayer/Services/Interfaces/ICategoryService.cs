using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace ServiceLayer.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync(string? search = null);
        Task<Category?> GetByIdAsync(short id);
        Task CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task<bool> DeleteAsync(short id);
        Task<bool> HasArticlesAsync(short id);
    }
}
