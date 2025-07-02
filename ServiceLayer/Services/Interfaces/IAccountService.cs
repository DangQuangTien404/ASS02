using System.Threading.Tasks;
using DAL.Entities;

namespace ServiceLayer.Interfaces
{
    public interface IAccountService
    {
        Task<SystemAccount?> AuthenticateAsync(string email, string password);
        Task<SystemAccount?> GetByIdAsync(short id);
        Task UpdateAsync(SystemAccount account);
        IEnumerable<SystemAccount> GetAllAccounts();
    }

}
