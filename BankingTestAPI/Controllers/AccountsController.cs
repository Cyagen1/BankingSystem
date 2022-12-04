using BankingTestAPI.DbContexts;
using BankingTestAPI.Models;
using BankingTestAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BankingTestAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountContextSqlServer _context;
        private readonly IAccountRepository _accountRepository;

        public AccountsController(AccountContextSqlServer context, IAccountRepository accountRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _accountRepository= accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> GetAccount([FromRoute] int id)
        {
            var account = await _accountRepository.GetAccount(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<AccountDto>> PostAccount([FromBody] AccountDto account)
        {
            await _accountRepository.CreateAccount(account);
            if (account.State < 100)
            {
                return BadRequest("Account state needs to have at least 100$");
            }

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAccount([FromRoute] int id)
        {
            var account = await _accountRepository.DeleteAccount(id);
            if (account == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpPatch("{id}/deposit")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Deposit([FromRoute] int id)
        {
            var account = await _context.Accounts.FindAsync(id);            

            if (account == null)
            {
                return NotFound();
            }

            var deposit = await _accountRepository.Deposit(account);
            if (deposit == null)
            {
                return BadRequest();
            }

            return NoContent();

        }

        [HttpPatch("{id}/withdraw")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Withdraw([FromRoute] int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            var withdrawal = await _accountRepository.Withdraw(account);
            if (withdrawal == null)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}