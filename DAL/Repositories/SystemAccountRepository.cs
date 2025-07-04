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

            // Fields always editable by the owner
            existing.AccountName = updated.AccountName;
            existing.AccountPassword = updated.AccountPassword;

            // Email and role can only be modified by an admin. The calling
            // code for admin edits supplies the new values whereas profile
            // updates reuse the existing entity so these remain unchanged.
            if (existing.AccountEmail != updated.AccountEmail)
                existing.AccountEmail = updated.AccountEmail;

            if (existing.AccountRole != updated.AccountRole)
                existing.AccountRole = updated.AccountRole;

            await _ctx.SaveChangesAsync();
        }

        public async Task AddAsync(SystemAccount account)
        {
            await _ctx.SystemAccount.AddAsync(account);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(short id)
        {
            var existing = await _ctx.SystemAccount.FindAsync(id);
            if (existing != null)
            {
                _ctx.SystemAccount.Remove(existing);
                await _ctx.SaveChangesAsync();
            }
        }

    }
}
