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
    public class CreateModel : PageModel
    {
        private readonly INewsService _news;
        private readonly ICategoryService _cats;
        private readonly NotificationService _notificationService;

        public CreateModel(INewsService news, ICategoryService cats, NotificationService notificationService)
        {
            _news = news;
            _cats = cats;
            _notificationService = notificationService;
        }

        [BindProperty]
        public NewsArticle Article { get; set; } = new();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new();

        public IEnumerable<SelectListItem> CategoryList { get; private set; }
            = Enumerable.Empty<SelectListItem>();

        public IList<Tag> AllTags { get; private set; } = new List<Tag>();

        public async Task OnGetAsync()
        {
            var cats = await _cats.GetAllAsync();
            CategoryList = cats
              .Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()));

            AllTags = (await _news.GetAllTagsAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    await OnGetAsync();
            //    return Page();
            //}

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

            // The CreateAsync method already persists changes, so no need to call SaveChangesAsync
            await _news.CreateAsync(Article, SelectedTagIds);
            
            // Send notification to all clients
            await _notificationService.NotifyAsync();

            return RedirectToPage("Index");
        }
    }
}
