namespace BankingTestAPI.Models
{
    public class AccountDto
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public int State { get; set; }
    }
}
