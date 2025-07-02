using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DangQuangTien_RazorPages.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _svc;
        public IndexModel(ICategoryService svc) => _svc = svc;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public IEnumerable<DAL.Entities.Category> Categories { get; set; }
            = new List<DAL.Entities.Category>();

        public int? UserRole { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            UserRole = HttpContext.Session.GetInt32("AccountRole");

            if (UserRole == null)
                return RedirectToPage("/Account/Login");

            if (UserRole != 1) // Only Staff can access Category management
                return Forbid();

            Categories = await _svc.GetAllAsync(SearchTerm);
            return Page();
        }
    }
}
