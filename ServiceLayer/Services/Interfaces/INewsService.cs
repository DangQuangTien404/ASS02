using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace ServiceLayer.Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> GetAllAsync(string? search = null, bool onlyActive = false);
        Task<NewsArticle?> GetByIdAsync(string id);
        Task CreateAsync(NewsArticle article, IEnumerable<int> tagIds);
        Task UpdateAsync(NewsArticle article, IEnumerable<int> tagIds);
        Task DeleteAsync(string id);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<IEnumerable<NewsArticle>> GetByAuthorIdAsync(short authorId);


        event Func<NewsArticle, Task>? OnArticlePublished;
    }
}
