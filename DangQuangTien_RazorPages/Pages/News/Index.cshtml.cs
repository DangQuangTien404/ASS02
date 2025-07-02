using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CategoryEntity = DAL.Entities.Category;

namespace DangQuangTien_RazorPages.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleRepository newsRepo;
        private readonly ICategoryRepository categoryRepo;
        private readonly NotificationService _notificationService;
        public IndexModel(INewsArticleRepository newsRepo, ICategoryRepository categoryRepo, NotificationService notificationService)
        {
            this.newsRepo = newsRepo;
            this.categoryRepo = categoryRepo;
            _notificationService = notificationService;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool OnlyActive { get; set; } = true;

        [BindProperty(SupportsGet = true)]
        public int? SelectedCategoryId { get; set; }

        public List<CategoryEntity> Categories { get; private set; } = new();

        public List<NewsArticle> Articles { get; private set; } = new();

        public bool CanEdit { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");

            if (role == null)
                return RedirectToPage("/Account/Login");

            if (role == 2)
            {
                OnlyActive = true;
                CanEdit = false;
            }
            else if (role == 1)
            {
                CanEdit = true;
            }
            else
            {
                return Forbid();
            }

            Categories = (await categoryRepo.GetAllAsync()).ToList();

            await LoadArticles();

            return Page();
        }

        public async Task<IActionResult> OnGetIndexPartial(string SearchTerm, int? SelectedCategoryId, bool OnlyActive)
        {
            // Set properties so they can be used in filtering
            this.SearchTerm = SearchTerm;
            this.SelectedCategoryId = SelectedCategoryId;
            this.OnlyActive = OnlyActive;

            // Re-use the same logic from OnGetAsync but without the Category loading
            await LoadArticles();

            // Return just the partial view
            return Partial("IndexPartial", this);
        }

        private async Task LoadArticles()
        {
            var articles = (await newsRepo.GetAllAsync(SearchTerm)).AsQueryable();
            
            if (SelectedCategoryId.HasValue)
                articles = articles.Where(a => a.CategoryId == SelectedCategoryId);
            
            if (OnlyActive)
                articles = articles.Where(a => a.NewsStatus == true);
            
            Articles = articles.OrderByDescending(a => a.CreatedDate).ToList();
        }
    }
}
