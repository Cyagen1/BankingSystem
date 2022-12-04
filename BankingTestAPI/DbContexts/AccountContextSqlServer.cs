using BankingTestAPI.Models;
using Microsoft.EntityFrameworkCore;

# nullable disable

namespace BankingTestAPI.DbContexts
{
    public partial class AccountContextSqlServer : DbContext
    {
       

        public AccountContextSqlServer(DbContextOptions<AccountContextSqlServer> options)
            : base(options)
        {
        }

        public DbSet<AccountDto> Accounts { get; set; }



    }
}
