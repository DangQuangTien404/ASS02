using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.Interfaces;

namespace DangQuangTien_RazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _svc;
        public LoginModel(IAccountService svc) => _svc = svc;

        [BindProperty]
        public LoginInput Input { get; set; }

        public class LoginInput
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _svc.AuthenticateAsync(Input.Email, Input.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return Page();
            }

            HttpContext.Session.SetString("AccountEmail", user.AccountEmail!);
            HttpContext.Session.SetInt32("AccountRole", user.AccountRole ?? -1);
            HttpContext.Session.SetInt32("AccountId", user.AccountId); 

            return RedirectToPage("/Index");
        }
    }
}
