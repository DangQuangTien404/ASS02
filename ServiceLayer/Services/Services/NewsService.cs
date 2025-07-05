using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

namespace ServiceLayer.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsArticleRepository _repo;
        public event Func<NewsArticle, Task>? OnArticlePublished;

        public NewsService(INewsArticleRepository repo)
            => _repo = repo;

        public async Task<IEnumerable<NewsArticle>> GetAllAsync(string? search = null, bool onlyActive = false)
        {
            var all = await _repo.GetAllAsync(search);
            return onlyActive
                ? all.Where(a => a.NewsStatus.GetValueOrDefault())
                : all;
        }

        public Task<NewsArticle?> GetByIdAsync(string id)
            => _repo.GetByIdAsync(id);

        public async Task CreateAsync(NewsArticle article, IEnumerable<int> tagIds)
        {
            article.NewsArticleId = Guid.NewGuid().ToString("N")[..20];
            article.CreatedDate = DateTime.UtcNow;
            article.NewsStatus = true;

            if (article.NewsSource == null)
                article.NewsSource = "N/A";

            await _repo.AddAsync(article);
            await _repo.SaveChangesAsync();

            await _repo.AddTagsToArticleAsync(article.NewsArticleId, tagIds);

            if (OnArticlePublished != null)
                await OnArticlePublished.Invoke(article);

        }


        public async Task UpdateAsync(NewsArticle article, IEnumerable<int> tagIds)
        {
            var existing = await _repo.GetByIdAsync(article.NewsArticleId);
            if (existing == null)
                throw new InvalidOperationException("Article not found.");

            existing.NewsTitle = article.NewsTitle;
            existing.Headline = article.Headline ?? article.NewsTitle ?? "Untitled";
            existing.NewsContent = article.NewsContent;
            existing.NewsSource = article.NewsSource; 
            existing.CategoryId = article.CategoryId;
            existing.ModifiedDate = DateTime.UtcNow;
            existing.UpdatedById = article.UpdatedById;

            await _repo.SaveChangesAsync();

            await _repo.RemoveAllTagsFromArticleAsync(article.NewsArticleId);
            await _repo.AddTagsToArticleAsync(article.NewsArticleId, tagIds);
        }

        public Task<IEnumerable<Tag>> GetAllTagsAsync()
            => _repo.GetAllTagsAsync();

        public async Task DeleteAsync(string id)
        {
            var article = await _repo.GetByIdAsync(id);

            if (article == null)
                return;

            await _repo.RemoveAllTagsFromArticleAsync(article.NewsArticleId);

            _repo.Remove(article);
            await _repo.SaveChangesAsync();
        }
        public Task<IEnumerable<NewsArticle>> GetByAuthorIdAsync(short authorId)
            => _repo.GetByAuthorIdAsync(authorId);

    }
}
