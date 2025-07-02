using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount?> GetByEmailAndPasswordAsync(string email, string password);
        Task<SystemAccount?> GetByIdAsync(short id);
        Task UpdateAsync(SystemAccount account);
        IEnumerable<SystemAccount> GetAll();
    }
}
