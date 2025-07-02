using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;

// ← Alias the scaffolded entity so “Category” in this file refers to the type
using CategoryEntity = DAL.Entities.Category;

namespace DangQuangTien_RazorPages.Pages.Category
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _svc;
        public CreateModel(ICategoryService svc) => _svc = svc;

        [BindProperty]
        public CategoryEntity Category { get; set; } = new CategoryEntity
        {
            // default new categories to active
            IsActive = true
        };

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _svc.CreateAsync(Category);
            return RedirectToPage("Index");
        }
    }
}
