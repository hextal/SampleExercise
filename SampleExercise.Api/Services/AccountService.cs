using Models.Dto;
using DataRepository.Interfaces;
using SampleExercise.Interfaces;

namespace SampleExercise.Services
{
    public class AccountService : IAccountService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentAccountRepository _currentAccountRepository;
        private readonly ILogger<AccountService> _logger;


        public AccountService(ICustomerRepository customerRepository, ICurrentAccountRepository currentAccountRepository, ILogger<AccountService> logger)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _currentAccountRepository = currentAccountRepository;
        }

        public async Task CreateCurrentAccount(Guid customerId, decimal initialDeposit)
        {
            var customer = await _customerRepository.GetCustomerById(customerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found with ID: {CustomerId}", customerId);
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

            if (customer.BankAccounts.Count == 0) return null;
            var accountId = customer.BankAccounts.First();
            var account = await _currentAccountRepository.GetAccount(accountId);
            if (account?.Transactions == null) return null;
            _logger.LogInformation("Retrieved account details for Account ID: {AccountId}", account.AccountId);
            account.Transactions = account.Transactions.OrderByDescending(t => t.TransactionDate).Take(3).ToList();
            return account;

        }
    }
}
