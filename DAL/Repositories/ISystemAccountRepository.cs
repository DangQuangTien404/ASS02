using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount?> GetByEmailAndPasswordAsync(string email, string password);
        Task<SystemAccount?> GetByIdAsync(short id);
        Task UpdateAsync(SystemAccount account);
        Task AddAsync(SystemAccount account);
        Task DeleteAsync(short id);
        IEnumerable<SystemAccount> GetAll();
    }
}
