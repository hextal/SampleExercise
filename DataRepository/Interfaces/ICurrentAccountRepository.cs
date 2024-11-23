using Models.Dto;
using System;
using System.Threading.Tasks;

namespace DataRepository.Interfaces
{
    public interface ICurrentAccountRepository
    {
        Task<CurrentAccountDto?> GetAccount(Guid accountId);
        Task<CurrentAccountDto> CreateAccount(Guid customerId, decimal initialDeposit);
        Task<CurrentAccountDto?> GetCustomerAccount(Guid customerId);
    }
}