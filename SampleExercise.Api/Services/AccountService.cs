using Models.Dto;
using DataRepository.Interfaces;
using SampleExercise.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SampleExercise.Services
{
    public class AccountService : IAccountService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentAccountRepository _currentAccountRepository;

        public AccountService(ICustomerRepository customerRepository, ICurrentAccountRepository currentAccountRepository)
        {
            _customerRepository = customerRepository;
            _currentAccountRepository = currentAccountRepository;
        }

        public async Task CreateCurrentAccount(Guid customerId, decimal initialDeposit)
        {
            var customer = await _customerRepository.GetCustomerById(customerId);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            var accountId = Guid.NewGuid();
            var account = new CurrentAccountDto
            {
                AccountId = accountId,
                Balance = initialDeposit,
                DateOpened = DateTime.UtcNow,
                Transactions = new System.Collections.Generic.List<TransactionDto>()
            };
        }

        public async Task<CurrentAccountDto?> GetCustomerAccountDetails(Guid customerId)
        {
            var customer = await _customerRepository.GetCustomerById(customerId);
            if (customer == null)
            {
                return null;
            }

            if (customer.BankAccounts != null && customer.BankAccounts.Any())
            {
                var accountId = customer.BankAccounts.First();
                var account = await _currentAccountRepository.GetAccount(accountId);
                if (account != null && account.Transactions != null)
                {
                    account.Transactions = account.Transactions.OrderByDescending(t => t.TransactionDate).Take(3).ToList();
                    return account;
                }
            }

            return null;
        }
    }
}
