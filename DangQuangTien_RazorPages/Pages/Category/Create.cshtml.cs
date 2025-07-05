using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;

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
            IsActive = true
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
                return Page();

            await _svc.CreateAsync(Category);
            return RedirectToPage("Index");
        }
    }
}
