using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount?> GetByEmailAndPasswordAsync(string email, string password);
        Task<SystemAccount?> GetByIdAsync(short id);
        /// <summary>
        /// Update an existing account. Name and password are always updated.
        /// When the supplied <see cref="SystemAccount"/> contains different
        /// <c>AccountEmail</c> or <c>AccountRole</c> values, these changes are
        /// applied as well. Normal profile edits reuse the existing values, so
        /// only administrators can modify email and role.
        /// </summary>
        Task UpdateAsync(SystemAccount account);
        Task AddAsync(SystemAccount account);
        Task DeleteAsync(short id);
        IEnumerable<SystemAccount> GetAll();
    }
}
