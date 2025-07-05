using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Interfaces;

namespace DangQuangTien_RazorPages.Pages.Account
{
    public class DeleteModel : PageModel
    {
        private readonly IAccountService _svc;
        public DeleteModel(IAccountService svc) => _svc = svc;

        [BindProperty]
        public SystemAccount? Account { get; set; }

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");

            Account = await _svc.GetByIdAsync(id);
            if (Account == null)
                return RedirectToPage("Index");

            return Page();
        }

        public async Task<IActionResult> OnGetFormAsync(short id)
        {
            var result = await OnGetAsync(id);
            if (result is PageResult)
                return Partial("_DeleteFormPartial", this);
            return result;
        }

        public async Task<IActionResult> OnPostAsync(short id)
        {
            var role = HttpContext.Session.GetInt32("AccountRole");
            if (role != 0)
                return RedirectToPage("/Account/AccessDenied");

            await _svc.DeleteAsync(id);

            return RedirectToPage("Index");
        }
    }
}
