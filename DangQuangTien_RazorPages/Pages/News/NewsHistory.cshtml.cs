using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using ServiceLayer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DangQuangTien_RazorPages.Pages.News
{
    public class NewsHistoryModel : PageModel
    {
        private readonly INewsService _svc;

        public NewsHistoryModel(INewsService svc) => _svc = svc;

        public IEnumerable<NewsArticleDto> Articles { get; set; } = new List<NewsArticleDto>();

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
