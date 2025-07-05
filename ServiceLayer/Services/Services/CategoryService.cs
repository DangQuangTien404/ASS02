using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;
using ServiceLayer.DTOs;
using ServiceLayer.Interfaces;

namespace ServiceLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
            => _repo = repo;

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(string? search = null)
        {
            var cats = await _repo.GetAllAsync(search);
            var result = new List<CategoryDto>();
            foreach (var c in cats)
            {
                result.Add(MapToDto(c));
            }
            return result;
        }

        public async Task<CategoryDto?> GetByIdAsync(short id)
        {
            var cat = await _repo.GetByIdAsync(id);
            return cat is null ? null : MapToDto(cat);
        }

        public async Task CreateAsync(CreateCategoryDto category)
        {
            var entity = new Category
            {
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateCategoryDto category)
        {
            var entity = new Category
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive
            };
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
        }

        public Task<bool> HasArticlesAsync(short id)
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

        private static CategoryDto MapToDto(Category c)
            => new()
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDesciption = c.CategoryDesciption,
                ParentCategoryId = c.ParentCategoryId,
                IsActive = c.IsActive
            };
    }
}
