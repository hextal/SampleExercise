using SampleExercise.Models;

namespace SampleExercise.Interfaces;

public interface IAccountService
{
    protected Task CreateCurrentAccount(Guid customerId, decimal initialDeposit);
    protected Task GetCustomerAccountDetails(Guid customerId);
}