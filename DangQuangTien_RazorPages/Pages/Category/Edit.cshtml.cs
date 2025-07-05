using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using CategoryEntity = DAL.Entities.Category;

namespace DangQuangTien_RazorPages.Pages.Category
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _svc;
        public EditModel(ICategoryService svc) => _svc = svc;

        [BindProperty]
        public CategoryEntity Category { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var category = await _svc.GetByIdAsync(id);
            if (category == null) return RedirectToPage("Index");
            Category = category;
            return Page();
        }

        public async Task<IActionResult> OnGetFormAsync(short id)
        {
            var result = await OnGetAsync(id);
            if (result is PageResult)
                return Partial("_EditFormPartial", this);
            return result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _svc.UpdateAsync(Category);
            return RedirectToPage("Index");
        }
    }
}
