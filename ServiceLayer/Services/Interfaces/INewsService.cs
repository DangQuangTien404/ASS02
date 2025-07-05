using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticleDto>> GetAllAsync(string? search = null, bool onlyActive = false);
        Task<NewsArticleDto?> GetByIdAsync(string id);
        Task CreateAsync(CreateNewsArticleDto article, IEnumerable<int> tagIds);
        Task UpdateAsync(UpdateNewsArticleDto article, IEnumerable<int> tagIds);
        Task DeleteAsync(string id);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<IEnumerable<NewsArticleDto>> GetByAuthorIdAsync(short authorId);


        event Func<NewsArticleDto, Task>? OnArticlePublished;
    }
}
