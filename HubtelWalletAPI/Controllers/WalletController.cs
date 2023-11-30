using System.Linq;
using System.Security.Claims;
using HubtelWalletAPI.Authentication;
using HubtelWalletAPI.BussinesLogic;
using HubtelWalletDatabase.DataAccess;
using HubtelWalletModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HubtelWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        readonly WalletDataAccess _context;

        public WalletController(WalletDataAccess context)
        {
            _context = context;
        }

        // GET: api/Wallets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetWallets()
        {
            var user = User;
            var userId = user.FindFirst(ClaimTypes.Name).Value;
            return await _context.GetWallets(userId);
        }

        // GET: api/Wallets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> GetWallet(int id)
        {
            var wallet = await _context.GetWallet(id);

            if (wallet == null)
            {
                return NotFound();
            }

            return wallet;
        }

        // POST: api/Wallets
        [HttpPost]
        public async Task<ActionResult<Wallet>> PostWallet(Wallet wallet)
        {
            var user = User;
            var userId = user.FindFirst(ClaimTypes.Name).Value;
            var result = await ValidateWallet.ValidateWalletAsync(wallet, _context, userId);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);
            wallet.CreatedAt = DateTime.Now;
            wallet.Owner = userId;

            _context.PostWallet(wallet);

            return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, wallet);
        }

        // DELETE: api/Wallets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            try
            {
                await _context.Delete(id);
            }
            catch(NullReferenceException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
