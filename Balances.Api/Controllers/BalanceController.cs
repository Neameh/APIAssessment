using Balances.Api.DTOs;
using Balances.Api.DTOs.ExternalBalanceService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Balances.Api.Controllers
{
    //public class BalanceController : Controller
    //{

    //    [HttpPost("api/balance/Debit")]
    //    public async Task<IActionResult> DebitBalance([FromBody]BalanceRequest debitRequest)
    //    {
    //        return Ok();
    //    }

    //    [HttpGet("api/balance/{userId}")]
    //    public async Task<IActionResult> GetBalance(int userId)
    //    {
    //        return Ok(1000);
    //    }
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Balances.Api.Models;

    namespace ExternalBalanceService.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class BalanceController : ControllerBase
        {
            private readonly BalanceDbContext _context;

            public BalanceController(BalanceDbContext context)
            {
                _context = context;
            }

            [HttpGet("{userId}")]
            public async Task<ActionResult<BalanceResponse>> GetBalance(int userId)
            {
                var balance = await _context.Balances.FirstOrDefaultAsync(b => b.UserId == userId);
                if (balance != null)
                {
                    return Ok(new BalanceResponse { UserId = userId, Balance = balance.Amount, Success = true });
                }
                return NotFound(new BalanceResponse { UserId = userId, Success = false, Message = "User not found." });
            }

            [HttpPost("debit")]
            public async Task<ActionResult<BalanceResponse>> DebitBalance([FromBody] BalanceRequest request)
            {
                var balance = await _context.Balances.FirstOrDefaultAsync(b => b.UserId == request.UserId);
                if (balance != null)
                {
                    if (balance.Amount < request.Amount)
                    {
                        return BadRequest(new BalanceResponse { UserId = request.UserId, Success = false, Message = "Insufficient balance." });
                    }

                    balance.Amount -= request.Amount;
                    await _context.SaveChangesAsync();
                    return Ok(new BalanceResponse { UserId = request.UserId, Balance = balance.Amount, Success = true });
                }
                return NotFound(new BalanceResponse { UserId = request.UserId, Success = false, Message = "User not found." });
            }

            [HttpPost("credit")]
            public async Task<ActionResult<BalanceResponse>> CreditBalance([FromBody] BalanceRequest request)
            {
                var balance = await _context.Balances.FirstOrDefaultAsync(b => b.UserId == request.UserId);
                if (balance != null)
                {
                    balance.Amount += request.Amount;
                    await _context.SaveChangesAsync();
                    return Ok(new BalanceResponse { UserId = request.UserId, Balance = balance.Amount, Success = true });
                }
                return NotFound(new BalanceResponse { UserId = request.UserId, Success = false, Message = "User not found." });
            }
        }
    }

}

