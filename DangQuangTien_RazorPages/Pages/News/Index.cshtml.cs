using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CategoryEntity = DAL.Entities.Category;

namespace DangQuangTien_RazorPages.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly INewsService _news;
        private readonly ICategoryService _cats;
        private readonly NotificationService _notificationService;
        public IndexModel(INewsService news, ICategoryService cats, NotificationService notificationService)
        {
            _news = news;
            _cats = cats;
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

            if (role == 1) // Staff can edit
            {
                CanEdit = true;
            }
            else // Lecturer, Admin or anonymous users
            {
                OnlyActive = true;
                CanEdit = false;
            }

            Categories = (await _cats.GetAllAsync()).ToList();

            await LoadArticles();

            return Page();
        }

        public async Task<IActionResult> OnGetIndexPartial(string SearchTerm, int? SelectedCategoryId, bool OnlyActive)
        {
            var role = HttpContext.Session.GetInt32("AccountRole");

            // Set properties so they can be used in filtering
            this.SearchTerm = SearchTerm;
            this.SelectedCategoryId = SelectedCategoryId;
            this.OnlyActive = OnlyActive;

            if (role != 1)
            {
                // Visitors and lecturers should only see active articles
                this.OnlyActive = true;
            }

            // Re-use the same logic from OnGetAsync but without the Category loading
            await LoadArticles();

            // Return just the partial view
            return Partial("IndexPartial", this);
        }

        private async Task LoadArticles()
        {
            var articles = (await _news.GetAllAsync(SearchTerm, OnlyActive)).AsQueryable();

            if (SelectedCategoryId.HasValue)
                articles = articles.Where(a => a.CategoryId == SelectedCategoryId);

            Articles = articles.OrderByDescending(a => a.CreatedDate).ToList();
        }
    }
}
