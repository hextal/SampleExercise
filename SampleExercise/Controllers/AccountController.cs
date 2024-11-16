using Microsoft.AspNetCore.Mvc;
using SampleExercise.Interfaces;

namespace SampleExercise.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpPost(Name = "CreateCurrentAccount")]
    public void Post(Guid customerId, decimal initialDeposit)
    {
    }

    [HttpGet(Name = "GetCustomerAccountDetails")]
    public void Get(Guid customerId)
    {
    }
}