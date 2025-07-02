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
    }
}
