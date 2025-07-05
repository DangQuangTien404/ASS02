using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using ServiceLayer.DTOs;
using System.Threading.Tasks;

namespace DangQuangTien_RazorPages.Pages.News
{
    public class DeleteModel : PageModel
    {
        private readonly INewsService _news;
        private readonly NotificationService _notificationService;
        public DeleteModel(INewsService news, NotificationService notificationService)
        {
            _news = news;
            _notificationService = notificationService;
        }

        [BindProperty]
        public NewsArticleDto? Article { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            Article = await _news.GetByIdAsync(id);
            if (Article == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnGetFormAsync(string id)
        {
            var result = await OnGetAsync(id);
            if (result is PageResult)
                return Partial("_DeleteFormPartial", this);
            return result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Article?.NewsArticleId))
                return NotFound();

            await _news.DeleteAsync(Article.NewsArticleId);
            await _notificationService.NotifyAsync();
            return RedirectToPage("Index");
        }
    }
}
