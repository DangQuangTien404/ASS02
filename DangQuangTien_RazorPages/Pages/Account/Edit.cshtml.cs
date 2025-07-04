using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Interfaces;

namespace DangQuangTien_RazorPages.Pages.Account
{
    public class EditModel : PageModel
    {
        private readonly IAccountService _svc;
        public EditModel(IAccountService svc) => _svc = svc;

        [BindProperty]
        public SystemAccount Account { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");

            var acc = await _svc.GetByIdAsync(id);
            if (acc == null)
                return RedirectToPage("Index");

            Account = acc;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");

            if (!ModelState.IsValid)
                return Page();

            var existing = await _svc.GetByIdAsync(Account.AccountId);
            if (existing == null)
                return RedirectToPage("Index");

            existing.AccountName = Account.AccountName;
            existing.AccountEmail = Account.AccountEmail;
            existing.AccountRole = Account.AccountRole;
            if (!string.IsNullOrWhiteSpace(Account.AccountPassword))
                existing.AccountPassword = Account.AccountPassword;

            await _svc.UpdateAsync(existing);
            return RedirectToPage("Index");
        }
    }
}
