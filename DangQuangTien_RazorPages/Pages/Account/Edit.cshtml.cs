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
        public SystemAccount Account { get; set; } = new();

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

        public async Task<IActionResult> OnGetFormAsync(short id)
        {
            var result = await OnGetAsync(id);
            if (result is PageResult)
                return Partial("_EditFormPartial", this);
            return result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");

            if (!ModelState.IsValid)
                return Page();

            await _svc.UpdateAsync(Account);
            return RedirectToPage("Index");
        }
    }
}
