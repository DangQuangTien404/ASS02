using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using DAL.Entities;
using DAL.Repositories;
using ServiceLayer.DTOs;
using ServiceLayer.Interfaces;

namespace ServiceLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly ISystemAccountRepository _repo;
        private readonly AdminAccountSettings _admin;

        public AccountService(
            ISystemAccountRepository repo,
            IOptions<AdminAccountSettings> adminOptions)
        {
            _repo = repo;
            _admin = adminOptions.Value;
        }
        public IEnumerable<SystemAccount> GetAllAccounts()
        {
            return _repo.GetAll();
        }
        public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
        {
            // 1) Check for Admin credentials from appsettings.json
            if (email == _admin.Email && password == _admin.Password)
            {
                return new SystemAccount
                {
                    AccountId = 0,
                    AccountName = "Administrator",
                    AccountEmail = _admin.Email,
                    AccountRole = 0,
                    AccountPassword = _admin.Password
                };
            }

            // 2) Otherwise fallback to DB (Staff=1, Lecturer=2)
            return await _repo.GetByEmailAndPasswordAsync(email, password);
        }
        public Task<SystemAccount?> GetByIdAsync(short id)
             => _repo.GetByIdAsync(id);

        public Task UpdateAsync(SystemAccount account)
            => _repo.UpdateAsync(account);

        public Task AddAsync(SystemAccount account)
            => _repo.AddAsync(account);

        public Task DeleteAsync(short id)
            => _repo.DeleteAsync(id);

    }
}
