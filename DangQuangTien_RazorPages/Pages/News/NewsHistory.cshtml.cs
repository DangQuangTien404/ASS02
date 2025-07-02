using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DangQuangTien_RazorPages.Pages.News
{
    public class NewsHistoryModel : PageModel
    {
        private readonly INewsService _svc;

        public NewsHistoryModel(INewsService svc) => _svc = svc;

        public IEnumerable<NewsArticle> Articles { get; set; } = new List<NewsArticle>();

        public async Task<IActionResult> OnGetAsync()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
                return RedirectToPage("/Account/Login");

            Articles = await _svc.GetByAuthorIdAsync((short)accountId);
            return Page();
        }
    }
}
