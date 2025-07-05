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

            existing.AccountName = updated.AccountName;
            existing.AccountPassword = updated.AccountPassword;

            if (existing.AccountEmail != updated.AccountEmail)
                existing.AccountEmail = updated.AccountEmail;

            if (existing.AccountRole != updated.AccountRole)
                existing.AccountRole = updated.AccountRole;

            await _ctx.SaveChangesAsync();
        }

        public async Task AddAsync(SystemAccount account)
        {
            // Assign a new unique ID when the database does not auto generate it
            if (account.AccountId == 0)
            {
                short nextId = 1;
                if (_ctx.SystemAccount.Any())
                {
                    nextId = (short)(_ctx.SystemAccount.Max(a => a.AccountId) + 1);
                }
                account.AccountId = nextId;
            }

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
