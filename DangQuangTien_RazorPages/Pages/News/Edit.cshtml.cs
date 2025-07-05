using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;


namespace DangQuangTien_RazorPages.Pages.News
{
    public class EditModel : NewsFormBase
    {
        public EditModel(INewsService news, ICategoryService cats, NotificationService notificationService)
            : base(news, cats, notificationService)
        {
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var article = await _news.GetByIdAsync(id);
            if (article == null)
                return NotFound();

            Article = article;
            SelectedTagIds = article.Tag.Select(t => t.TagId).ToList();

            await LoadCategoriesAndTagsAsync();

            return Page();
        }

        public async Task<IActionResult> OnGetFormAsync(string id)
        {
            var result = await OnGetAsync(id);
            if (result is PageResult)
                return Partial("_EditFormPartial", this);
            return result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(Article.NewsArticleId); 
                return Page();
            }

            Article.Headline ??= Article.NewsTitle ?? "Untitled";
            Article.ModifiedDate = DateTime.UtcNow;
            Article.UpdatedById = (short?)HttpContext.Session.GetInt32("AccountId");

            await _news.UpdateAsync(Article, SelectedTagIds);
            await _notificationService.NotifyAsync();
            return RedirectToPage("Index");
        }
    }
}
