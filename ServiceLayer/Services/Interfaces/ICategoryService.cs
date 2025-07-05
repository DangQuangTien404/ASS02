using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(string? search = null);
        Task<CategoryDto?> GetByIdAsync(short id);
        Task CreateAsync(CreateCategoryDto category);
        Task UpdateAsync(UpdateCategoryDto category);
        Task<bool> DeleteAsync(short id);
        Task<bool> HasArticlesAsync(short id);
    }
}
