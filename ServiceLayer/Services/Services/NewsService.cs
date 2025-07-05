using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsArticleRepository _repo;
        public event Func<NewsArticleDto, Task>? OnArticlePublished;

        public NewsService(INewsArticleRepository repo)
            => _repo = repo;

        public async Task<IEnumerable<NewsArticleDto>> GetAllAsync(string? search = null, bool onlyActive = false)
        {
            var all = await _repo.GetAllAsync(search);
            var filtered = onlyActive
                ? all.Where(a => a.NewsStatus.GetValueOrDefault())
                : all;
            return filtered.Select(MapToDto);
        }

        public async Task<NewsArticleDto?> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task CreateAsync(CreateNewsArticleDto article, IEnumerable<int> tagIds)
        {
            var entity = new NewsArticle
            {
                NewsArticleId = article.NewsArticleId,
                NewsTitle = article.NewsTitle,
                Headline = article.Headline ?? article.NewsTitle ?? "Untitled",
                CreatedDate = article.CreatedDate,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource ?? "N/A",
                CategoryId = article.CategoryId,
                NewsStatus = article.NewsStatus,
                CreatedById = article.CreatedById
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            await _repo.AddTagsToArticleAsync(entity.NewsArticleId, tagIds);

            if (OnArticlePublished != null)
                await OnArticlePublished.Invoke(MapToDto(entity));

        }

        public async Task UpdateAsync(UpdateNewsArticleDto article, IEnumerable<int> tagIds)
        {
            var existing = await _repo.GetByIdAsync(article.NewsArticleId);
            if (existing == null)
                throw new InvalidOperationException("Article not found.");

            existing.NewsTitle = article.NewsTitle;
            existing.Headline = article.Headline ?? article.NewsTitle ?? "Untitled";
            existing.NewsContent = article.NewsContent;
            existing.NewsSource = article.NewsSource;
            existing.CategoryId = article.CategoryId;
            existing.ModifiedDate = article.ModifiedDate;
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
        public async Task<IEnumerable<NewsArticleDto>> GetByAuthorIdAsync(short authorId)
        {
            var list = await _repo.GetByAuthorIdAsync(authorId);
            return list.Select(MapToDto);
        }

        private static NewsArticleDto MapToDto(NewsArticle a)
            => new()
            {
                NewsArticleId = a.NewsArticleId,
                NewsTitle = a.NewsTitle,
                Headline = a.Headline,
                CreatedDate = a.CreatedDate,
                NewsContent = a.NewsContent,
                NewsSource = a.NewsSource,
                CategoryId = a.CategoryId,
                NewsStatus = a.NewsStatus,
                CreatedById = a.CreatedById,
                UpdatedById = a.UpdatedById,
                ModifiedDate = a.ModifiedDate,
                Category = a.Category == null ? null : new CategoryDto
                {
                    CategoryId = a.Category.CategoryId,
                    CategoryName = a.Category.CategoryName,
                    CategoryDesciption = a.Category.CategoryDesciption,
                    ParentCategoryId = a.Category.ParentCategoryId,
                    IsActive = a.Category.IsActive
                },
                Tag = a.Tag.ToList()
            };

    }
}
