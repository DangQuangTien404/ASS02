using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Contexts;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly AppDbContext _ctx;
        public NewsArticleRepository(AppDbContext ctx) => _ctx = ctx;

        public Task<IEnumerable<NewsArticle>> GetAllAsync(string? search)
            => Task.FromResult(
                _ctx.NewsArticle
                    .Include(n => n.Category)
                    .Include(n => n.Tag) 
                    .Where(n => string.IsNullOrWhiteSpace(search)
                             || EF.Functions.Like(n.NewsTitle!, $"%{search}%"))
                    .AsEnumerable()
            );

        public Task<NewsArticle?> GetByIdAsync(string id)
            => _ctx.NewsArticle
                .Include(n => n.Category)
                .Include(n => n.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);

        public Task AddAsync(NewsArticle entity)
            => _ctx.NewsArticle.AddAsync(entity).AsTask();

        public void Update(NewsArticle entity)
            => _ctx.NewsArticle.Update(entity);

        public void Remove(NewsArticle entity)
            => _ctx.NewsArticle.Remove(entity);

        public Task SaveChangesAsync()
            => _ctx.SaveChangesAsync();

        public async Task AddTagsToArticleAsync(string newsArticleId, IEnumerable<int> tagIds)
        {
            if (string.IsNullOrWhiteSpace(newsArticleId) || tagIds == null)
                throw new ArgumentException("Invalid input for article ID or tag IDs.");

            var article = await _ctx.NewsArticle
                .Include(na => na.Tag)
                .FirstOrDefaultAsync(na => na.NewsArticleId == newsArticleId);

            if (article == null)
                throw new InvalidOperationException("News article not found.");

            var existingTagIds = article.Tag.Select(t => t.TagId).ToHashSet();

            var newTagIds = tagIds.Except(existingTagIds).ToList();

            if (newTagIds.Count > 0)
            {
                var tagsToAdd = await _ctx.Tag
                    .Where(t => newTagIds.Contains(t.TagId))
                    .ToListAsync();

                foreach (var tag in tagsToAdd)
                {
                    article.Tag.Add(tag);
                }

                await _ctx.SaveChangesAsync();
            }
        }

        public async Task RemoveAllTagsFromArticleAsync(string newsArticleId)
        {
            var article = await _ctx.NewsArticle
                .Include(n => n.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleId == newsArticleId);

            if (article == null)
                throw new InvalidOperationException("Article not found.");

            article.Tag.Clear(); 
            await _ctx.SaveChangesAsync();
        }

        public Task<IEnumerable<Tag>> GetAllTagsAsync()
            => Task.FromResult(_ctx.Tag.AsEnumerable());
        public async Task<IEnumerable<NewsArticle>> GetByAuthorIdAsync(short authorId)
        {
            return await _ctx.NewsArticle
                .Include(n => n.Category)
                .Include(n => n.Tag)
                .Where(n => n.CreatedById == authorId)
                .ToListAsync();
        }

    }
}
