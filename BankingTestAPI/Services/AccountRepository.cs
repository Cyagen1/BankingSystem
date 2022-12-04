using BankingTestAPI.DbContexts;
using BankingTestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingTestAPI.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountContextSqlServer _context;

        public AccountRepository(AccountContextSqlServer context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Method to return an Account by its Id.
        public async Task<AccountDto?> GetAccount(int id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        // Method to return all Accounts.
        public async Task<IEnumerable<AccountDto>> GetAllAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // Method to create and add a new Account to the database.
        public async Task<AccountDto> CreateAccount(AccountDto account)
        {
            await _context.Accounts.AddAsync(account);
            var limit = 100;
            if (account.State >= limit)
            {
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cannot make account with state under limit.");
            }
            return account;
        }

        // Method to delete an Account from the database.
        public async Task<AccountDto?> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            if (account != null)
            {
                _context.Remove(account);
                await _context.SaveChangesAsync();
            }
            return account;
        }

        // Method to make a deposit to the account, but only if the deposit is less than limit.
        public async Task<AccountDto?> Deposit(AccountDto accountChanges)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == accountChanges.Id);
            var limit = 10_000;
            if (account != null)
            {
                if(accountChanges.State > 0 && accountChanges.State <= limit)
                {
                    account.State = accountChanges.State;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Cannot deposit more than limit.");
                }
            }

            return account;
        }

        // Method to make a withdrawal from account, but only if the withdrawal is less than limit.
        public async Task<AccountDto?> Withdraw(AccountDto accountChanges)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == accountChanges.Id);
            
            if (account != null)
            {
                var limit = account.State * 0.9;
                if (accountChanges.State > 0 && accountChanges.State < limit)
                {
                    account.State = accountChanges.State;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Cannot withdraw more than limit.");
                }
            }
            return account;
        }
    }
}
