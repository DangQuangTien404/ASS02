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
    public abstract class NewsFormBase : PageModel
    {
        protected readonly INewsService _news;
        protected readonly ICategoryService _cats;
        protected readonly NotificationService _notificationService;

        [BindProperty]
        public NewsArticle Article { get; set; } = new();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new();

        public IEnumerable<SelectListItem> CategoryList { get; set; } = Enumerable.Empty<SelectListItem>();

        public IList<Tag> AllTags { get; set; } = new List<Tag>();

        protected NewsFormBase(INewsService news, ICategoryService cats, NotificationService notificationService)
        {
            _news = news;
            _cats = cats;
            _notificationService = notificationService;
        }

        protected async Task LoadCategoriesAndTagsAsync()
        {
            var cats = await _cats.GetAllAsync();
            CategoryList = cats.Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()));
            AllTags = (await _news.GetAllTagsAsync()).ToList();
        }
    }
}
