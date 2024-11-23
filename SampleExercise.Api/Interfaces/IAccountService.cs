using Models.Dto;

namespace SampleExercise.Interfaces
{
    public interface IAccountService
    {
        Task CreateCurrentAccount(Guid customerId, decimal initialDeposit);
        Task<CurrentAccountDto?> GetCustomerAccountDetails(Guid customerId);
    }
}