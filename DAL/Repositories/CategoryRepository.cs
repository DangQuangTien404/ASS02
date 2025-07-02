using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Contexts;
using DAL.Entities;

namespace DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _ctx;
        public CategoryRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Category>> GetAllAsync(string? search = null)
        {
            var query = _ctx.Category.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.CategoryName!, $"%{search}%"));
            }
            return await query
                .OrderByDescending(c => c.CategoryId)
                .ToListAsync();
        }
        public Task<Category?> GetByIdAsync(short categoryId)
            => _ctx.Category.FindAsync(categoryId).AsTask();

        public async Task AddAsync(Category category)
            => await _ctx.Category.AddAsync(category);

        public void Update(Category category)
            => _ctx.Category.Update(category);

        public void Remove(Category category)
            => _ctx.Category.Remove(category);

        public Task<bool> HasArticlesAsync(short categoryId)
            => _ctx.NewsArticle.AnyAsync(n => n.CategoryId == categoryId);

        public Task SaveChangesAsync()
            => _ctx.SaveChangesAsync();
    }
}
