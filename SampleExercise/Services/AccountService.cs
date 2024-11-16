using SampleExercise.Interfaces;
using SampleExercise.Models;

namespace SampleExercise.Services;

public class AccountService : IAccountService
{
    public Task CreateCurrentAccount(Guid customerId, decimal initialDeposit)
    {
        throw new NotImplementedException();
    }

    public Task GetCustomerAccountDetails(Guid customerId)
    {
        throw new NotImplementedException();
    }
}