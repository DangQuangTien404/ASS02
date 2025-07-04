using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using CategoryEntity = DAL.Entities.Category;

namespace DangQuangTien_RazorPages.Pages.Category
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _svc;
        public DeleteModel(ICategoryService svc) => _svc = svc;

        [BindProperty]
        public CategoryEntity? Category { get; set; }

        public async Task<IActionResult> OnGetAsync(short id)  
        {
            Category = await _svc.GetByIdAsync(id);
            if (Category == null) return RedirectToPage("Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short id) 
        {
            var success = await _svc.DeleteAsync(id);
            if (!success)
            {
                ModelState.AddModelError(string.Empty,
                    "Cannot delete: this category is in use.");
                Category = await _svc.GetByIdAsync(id);
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
