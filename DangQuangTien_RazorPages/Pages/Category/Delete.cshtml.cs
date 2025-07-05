using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using ServiceLayer.DTOs;

namespace DangQuangTien_RazorPages.Pages.Category
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _svc;
        public DeleteModel(ICategoryService svc) => _svc = svc;

        [BindProperty]
        public CategoryDto? Category { get; set; }
        public bool CanDelete { get; set; }

        public async Task<IActionResult> OnGetAsync(short id)
        {
            Category = await _svc.GetByIdAsync(id);
            if (Category == null) return RedirectToPage("Index");
            CanDelete = !await _svc.HasArticlesAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnGetFormAsync(short id)
        {
            Category = await _svc.GetByIdAsync(id);
            if (Category == null) return RedirectToPage("Index");
            CanDelete = !await _svc.HasArticlesAsync(id);
            return Partial("_DeleteFormPartial", this);
        }

        public async Task<IActionResult> OnPostAsync(short id)
        {
            var success = await _svc.DeleteAsync(id);
            if (!success)
            {
                const string msg =
                    "This category cannot be deleted because it is currently used in one or more news articles.";

                Category = await _svc.GetByIdAsync(id);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return new JsonResult(new { error = msg });

                ModelState.AddModelError(string.Empty, msg);
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
