using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;
using ServiceLayer.Interfaces;

namespace ServiceLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
            => _repo = repo;

        public Task<IEnumerable<Category>> GetAllAsync(string? search = null)
            => _repo.GetAllAsync(search);

        public Task<Category?> GetByIdAsync(short id)
            => _repo.GetByIdAsync(id);

        public async Task CreateAsync(Category category)
        {
            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _repo.Update(category);
            await _repo.SaveChangesAsync();
        }

        public Task<bool> IsInUseAsync(short id)
            => _repo.HasArticlesAsync(id);

        public async Task<bool> DeleteAsync(short id)
        {
            if (await _repo.HasArticlesAsync(id))
                return false;

            var cat = await _repo.GetByIdAsync(id);
            if (cat is null) return false;

            _repo.Remove(cat);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
