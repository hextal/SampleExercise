using DataRepository.Interfaces;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataRepository.Repositories
{
    public class CurrentAccountRepository : ICurrentAccountRepository
    {
        private static readonly List<CurrentAccountDto> _accounts = new List<CurrentAccountDto>();

        public async Task<CurrentAccountDto?> GetAccount(Guid accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountId == accountId);
            return await Task.FromResult(account);
        }

        public async Task<CurrentAccountDto> CreateAccount(Guid customerId, decimal initialDeposit)
        {
            var newAccount = new CurrentAccountDto
            {
                AccountId = Guid.NewGuid(),
                CustomerId = customerId,
                Balance = initialDeposit,
                DateOpened = DateTime.UtcNow,
                Transactions = new List<TransactionDto>()
            };

            _accounts.Add(newAccount);
            return await Task.FromResult(newAccount);
        }

        public async Task<CurrentAccountDto?> GetCustomerAccount(Guid customerId)
        {
            var account = _accounts.FirstOrDefault(a => a.CustomerId == customerId);
            return await Task.FromResult(account);
        }

        public async Task<CurrentAccountDto?> UpdateAccount(Guid accountId, decimal newBalance)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null) return await Task.FromResult<CurrentAccountDto?>(null);

            account.Balance = newBalance;
            return await Task.FromResult(account);
        }

        public async Task<bool> DeleteAccount(Guid accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null) return await Task.FromResult(false);

            _accounts.Remove(account);
            return await Task.FromResult(true);
        }
    }
}
