using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllAsync(string? search);
        Task<NewsArticle?> GetByIdAsync(string id);
        Task AddAsync(NewsArticle entity);
        void Update(NewsArticle entity);
        void Remove(NewsArticle entity);
        Task SaveChangesAsync();

        // many‐to‐many helpers
        Task AddTagsToArticleAsync(string newsArticleId, IEnumerable<int> tagIds);
        Task RemoveAllTagsFromArticleAsync(string newsArticleId);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<IEnumerable<NewsArticle>> GetByAuthorIdAsync(short authorId);

    }
}
