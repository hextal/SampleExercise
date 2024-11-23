using Microsoft.AspNetCore.Mvc;
using SampleExercise.Interfaces;
using Models.Dto;
using System;
using System.Threading.Tasks;

namespace SampleExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurrentAccount(Guid customerId, decimal initialDeposit)
        {
            try
            {
                await _accountService.CreateCurrentAccount(customerId, initialDeposit);
                return Ok(new { message = "Account created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<CurrentAccountDto>> GetCustomerAccountDetails(Guid customerId)
        {
            try
            {
                var account = await _accountService.GetCustomerAccountDetails(customerId);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found for the given customer." });
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}