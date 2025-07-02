using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(string? search = null);
        Task<Category?> GetByIdAsync(short categoryId);
        Task AddAsync(Category category);
        void Update(Category category);
        void Remove(Category category);
        Task<bool> HasArticlesAsync(short categoryId);
        Task SaveChangesAsync();
    }
}
