using BankingTestAPI.Models;

namespace BankingTestAPI.Services
{
    public interface IAccountRepository
    {
        Task<AccountDto?> GetAccount(int id);

        Task<IEnumerable<AccountDto>> GetAllAccounts();

        Task<AccountDto?> Deposit(AccountDto account);

        Task<AccountDto?> Withdraw(AccountDto account);

        Task<AccountDto?> DeleteAccount(int id);

        Task<AccountDto> CreateAccount(AccountDto account);
    }
}
