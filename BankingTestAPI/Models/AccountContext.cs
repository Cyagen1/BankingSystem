using Microsoft.EntityFrameworkCore;

namespace BankingTestAPI.Models
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }
        public DbSet<AccountDto> Accounts { get; set; } = null!;
    }
}
