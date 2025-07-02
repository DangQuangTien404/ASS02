using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Contexts;
using DAL.Entities;

namespace DAL.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly AppDbContext _ctx;
        public SystemAccountRepository(AppDbContext ctx)
            => _ctx = ctx;

        public async Task<SystemAccount?> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await _ctx.SystemAccount
                .FirstOrDefaultAsync(a =>
                    a.AccountEmail == email &&
                    a.AccountPassword == password);
        }
        public async Task<SystemAccount?> GetByIdAsync(short id)
        {
            return await _ctx.SystemAccount.FindAsync(id);
        }
        public IEnumerable<SystemAccount> GetAll()
        {
            return _ctx.SystemAccount.ToList();
        }
        public async Task UpdateAsync(SystemAccount updated)
        {
            var existing = await _ctx.SystemAccount.FindAsync(updated.AccountId);
            if (existing == null)
                throw new DbUpdateConcurrencyException("Account no longer exists.");

            // Update only fields allowed to change
            existing.AccountName = updated.AccountName;
            existing.AccountPassword = updated.AccountPassword;
            // Avoid changing Email or Role unless admin context

            await _ctx.SaveChangesAsync();
        }

    }
}
