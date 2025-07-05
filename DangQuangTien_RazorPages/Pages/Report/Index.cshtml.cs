using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Services;
using ServiceLayer.DTOs;

namespace DangQuangTien_RazorPages.Pages.Report
{
    public class IndexModel : PageModel
    {
        private readonly INewsService _news;

        public IndexModel(INewsService news)
        {
            _news = news;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public List<NewsArticleDto> NewsStats { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role == null || role != 0)
                return RedirectToPage("/Account/Login");

            var allNews = await _news.GetAllAsync();

            NewsStats = allNews
                .Where(n =>
                    (!StartDate.HasValue || n.CreatedDate >= StartDate) &&
                    (!EndDate.HasValue || n.CreatedDate <= EndDate))
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            return Page();
        }
    }
}
