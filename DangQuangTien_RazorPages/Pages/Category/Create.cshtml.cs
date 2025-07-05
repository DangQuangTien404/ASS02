using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using ServiceLayer.DTOs;

namespace DangQuangTien_RazorPages.Pages.Category
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _svc;
        public CreateModel(ICategoryService svc) => _svc = svc;

        [BindProperty]
        public CreateCategoryDto Category { get; set; } = new()
        {
            CategoryName = string.Empty,
            CategoryDesciption = string.Empty
        };

        public void OnGet() { }

        public IActionResult OnGetForm()
        {
            OnGet();
            return Partial("_CreateFormPartial", this);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Partial("_CreateFormPartial", this);
                return Page();
            }

            await _svc.CreateAsync(Category);
            return RedirectToPage("Index");
        }
    }
}
