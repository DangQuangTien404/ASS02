using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DangQuangTien_RazorPages.Pages.Report
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleRepository _newsRepo;

        public IndexModel(INewsArticleRepository newsRepo)
        {
            _newsRepo = newsRepo;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public List<NewsArticle> NewsStats { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role == null || role != 0)
                return RedirectToPage("/Account/Login");

            var allNews = await _newsRepo.GetAllAsync(null);

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
