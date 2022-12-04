using System.ComponentModel.DataAnnotations;

namespace BankingTestAPI.Models
{
    public class AccountDto
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        [Required]
        public int State { get; set; }
    }
}
