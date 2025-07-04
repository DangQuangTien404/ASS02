using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Interfaces;

namespace DangQuangTien_RazorPages.Pages.Account
{
    public class CreateModel : PageModel
    {
        private readonly IAccountService _svc;
        public CreateModel(IAccountService svc) => _svc = svc;

        [BindProperty]
        public SystemAccount Account { get; set; } = new SystemAccount();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");

            if (!ModelState.IsValid)
                return Page();

            await _svc.AddAsync(Account);
            return RedirectToPage("Index");
        }
    }
}
