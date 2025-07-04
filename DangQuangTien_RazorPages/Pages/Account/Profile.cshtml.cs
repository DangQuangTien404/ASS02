using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DangQuangTien_RazorPages.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly IAccountService _svc;
        public ProfileModel(IAccountService svc) => _svc = svc;

        [BindProperty]
        public SystemAccount Account { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var id = HttpContext.Session.GetInt32("AccountId");
            var role = HttpContext.Session.GetInt32("AccountRole");

            if (id == null || role != 1)
                return RedirectToPage("/Account/AccessDenied");

            var acc = await _svc.GetByIdAsync((short)id);
            if (acc == null)
                return RedirectToPage("/Account/Login");

            Account = acc;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = HttpContext.Session.GetInt32("AccountId");
            var role = HttpContext.Session.GetInt32("AccountRole");

            if (id == null || role != 1)
                return RedirectToPage("/Account/AccessDenied");

            if (!ModelState.IsValid)
                return Page();

            var existing = await _svc.GetByIdAsync((short)id);
            if (existing == null)
                return RedirectToPage("/Account/Login");

            existing.AccountName = string.IsNullOrWhiteSpace(Account.AccountName)
                                    ? existing.AccountName
                                    : Account.AccountName;

            existing.AccountPassword = string.IsNullOrWhiteSpace(Account.AccountPassword)
                                        ? existing.AccountPassword
                                        : Account.AccountPassword;

            await _svc.UpdateAsync(existing);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage("/Account/Profile");
        }
    }
}
