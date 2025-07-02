using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using System.Threading.Tasks;

namespace DangQuangTien_RazorPages.Pages.News
{
    public class DeleteModel : PageModel
    {
        private readonly INewsService _news;

        public DeleteModel(INewsService news)
        {
            _news = news;
        }

        [BindProperty]
        public NewsArticle? Article { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            Article = await _news.GetByIdAsync(id);
            if (Article == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Article?.NewsArticleId))
                return NotFound();

            await _news.DeleteAsync(Article.NewsArticleId);
            return RedirectToPage("Index");
        }
    }
}
