using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangQuangTien_RazorPages.Pages.News
{
    public class CreateModel : NewsFormBase
    {
        public CreateModel(INewsService news, ICategoryService cats, NotificationService notificationService)
            : base(news, cats, notificationService)
        {
        }

        public async Task OnGetAsync()
        {
            await LoadCategoriesAndTagsAsync();
        }

        public async Task<IActionResult> OnGetFormAsync()
        {
            await OnGetAsync();
            return Partial("_CreateFormPartial", this);
        }

        public async Task<IActionResult> OnPostAsync()
        {

            int? accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
            {
                ModelState.AddModelError("", "You must be logged in to create an article.");
                await OnGetAsync();
                return Page();
            }

            Article.NewsArticleId = Guid.NewGuid().ToString("N")[..20]; 
            Article.CreatedById = (short)accountId;
            Article.CreatedDate = DateTime.UtcNow;
            Article.NewsStatus = true;
            Article.Headline ??= Article.NewsTitle ?? "Untitled";

            await _news.CreateAsync(Article, SelectedTagIds);

            await _notificationService.NotifyAsync();

            return RedirectToPage("Index");
        }
    }
}
