using BankingTestAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BankingTestAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountContext _context;

        public AccountsController(AccountContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }


        [HttpPost]
        public async Task<ActionResult<AccountDto>> PostAccount(AccountDto account)
        {
            _context.Add(account);
            if (account.State < 100)
            {
                return BadRequest("Account state needs to have at least 100$");
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NoContent();
            }

            _context.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/deposit")]
        public async Task<IActionResult> Deposit(JsonPatchDocument<AccountDto> patchDocument, int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            int limit = 10000;

            if (account == null)
            {
                return NotFound();
            }

            int currentState = account.State;

            patchDocument.ApplyTo(account, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (account.State - currentState > limit)
            {
                return BadRequest($"Deposit cannot be more than {limit}.");
            }

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPatch("{id}/withdraw")]
        public async Task<IActionResult> Withdraw([FromRoute] int id, [FromBody] Withdrawal withdrawal)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            if (withdrawal.Value > account.State * 0.9)
            {
                return BadRequest($"Withdrawal too high.");
            }

            account.State -= withdrawal.Value;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}