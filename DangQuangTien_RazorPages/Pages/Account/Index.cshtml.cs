using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;

namespace DangQuangTien_RazorPages.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public List<SystemAccount> Accounts { get; set; }

        [BindProperty]
        public SystemAccount NewAccount { get; set; } = new SystemAccount();

        [BindProperty]
        public SystemAccount EditAccount { get; set; } = new SystemAccount();

        public IActionResult OnGet()
        {
            var email = HttpContext.Session.GetString("AccountEmail");
            var role = HttpContext.Session.GetInt32("AccountRole");

            if (string.IsNullOrEmpty(email))
                return RedirectToPage("/Account/Login", new { ReturnUrl = "/Account" });

            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");

            Accounts = _accountService.GetAllAccounts().ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");

            if (!ModelState.IsValid)
            {
                Accounts = _accountService.GetAllAccounts().ToList();
                return Page();
            }

            await _accountService.AddAsync(NewAccount);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");

            if (!ModelState.IsValid)
            {
                Accounts = _accountService.GetAllAccounts().ToList();
                return Page();
            }

            await _accountService.UpdateAsync(EditAccount);
            return RedirectToPage();
        }
    }
}
