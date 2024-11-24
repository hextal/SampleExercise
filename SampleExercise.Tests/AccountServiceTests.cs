using Moq;
using DataRepository.Interfaces;
using Microsoft.Extensions.Logging;
using Models.Dto;
using SampleExercise.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace SampleExercise.Tests
{
    public class AccountServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepoMock;
        private readonly Mock<ICurrentAccountRepository> _accountRepoMock;
        private readonly Mock<ILogger<AccountService>> _mockLogger;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _mockLogger = new Mock<ILogger<AccountService>>();
            _customerRepoMock = new Mock<ICustomerRepository>();
            _accountRepoMock = new Mock<ICurrentAccountRepository>();
            _accountService = new AccountService(_customerRepoMock.Object, _accountRepoMock.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateCurrentAccount_ThrowsException_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _customerRepoMock.Setup(repo => repo.GetCustomerById(customerId))
                .ReturnsAsync((CustomerDto)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _accountService.CreateCurrentAccount(customerId, 1000m));

            Assert.Equal("Customer not found", exception.Message);

            // Verify logger was called
            _mockLogger.Verify(logger =>
                    logger.Log(
                        LogLevel.Warning,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>(
                            (v, t) => v.ToString().Contains($"Customer not found with ID: {customerId}")),
                        null,
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCustomerAccountDetails_ReturnsNull_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _customerRepoMock.Setup(repo => repo.GetCustomerById(customerId))
                .ReturnsAsync((CustomerDto)null);

            // Act
            var result = await _accountService.GetCustomerAccountDetails(customerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCustomerAccountDetails_ReturnsNull_WhenCustomerHasNoAccounts()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new CustomerDto
            {
                CustomerId = customerId,
                BankAccounts = new List<Guid>(), // No accounts
                FirstName = "John",
                LastName = "Doe",
                CustomerAddress = "123 Main St",
                CustomerEmail = "john.doe@example.com",
                CustomerPhone = "123-456-7890"
            };

            _customerRepoMock.Setup(repo => repo.GetCustomerById(customerId))
                .ReturnsAsync(customer);

            // Act
            var result = await _accountService.GetCustomerAccountDetails(customerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCustomerAccountDetails_ReturnsAccountWithTransactions_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var transactions = new List<TransactionDto>
            {
                new TransactionDto
                {
                    TransactionId = Guid.NewGuid(),
                    Amount = 25,
                    TransactionDate = DateTime.UtcNow.AddDays(-2),
                    TransactionType = "Withdrawal",
                    TransactionStatus = "Success"
                },
                new TransactionDto
                {
                    TransactionId = Guid.NewGuid(),
                    Amount = 10,
                    TransactionDate = DateTime.UtcNow.AddDays(-3),
                    TransactionType = "Deposit",
                    TransactionStatus = "Success"
                },
                new TransactionDto
                {
                    TransactionId = Guid.NewGuid(),
                    Amount = 50,
                    TransactionDate = DateTime.UtcNow.AddDays(-1),
                    TransactionType = "Deposit",
                    TransactionStatus = "Pending"
                },
                new TransactionDto
                {
                    TransactionId = Guid.NewGuid(),
                    Amount = 100,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "Deposit",
                    TransactionStatus = "Success"
                }
            };

            var customer = new CustomerDto
            {
                CustomerId = customerId,
                BankAccounts = new List<Guid> { accountId },
                FirstName = "John",
                LastName = "Doe",
                CustomerAddress = "123 Main St",
                CustomerEmail = "john.doe@example.com",
                CustomerPhone = "123-456-7890"
            };

            var account = new CurrentAccountDto
            {
                AccountId = accountId,
                Balance = 1000m,
                DateOpened = DateTime.UtcNow.AddDays(-10),
                Transactions = transactions
            };

            _customerRepoMock.Setup(repo => repo.GetCustomerById(customerId))
                .ReturnsAsync(customer);

            _accountRepoMock.Setup(repo => repo.GetAccount(accountId))
                .ReturnsAsync(account);

            // Act
            var result = await _accountService.GetCustomerAccountDetails(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Transactions.Count); // Should only return the latest 3 transactions
            Assert.Equal(100, result.Transactions.First().Amount);

            // Verify logger was called
            _mockLogger.Verify(logger =>
                    logger.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) =>
                            v.ToString().Contains($"Retrieved account details for Account ID: {accountId}")),
                        null,
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCustomerAccountDetails_ReturnsNull_WhenAccountHasNoTransactions()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var accountId = Guid.NewGuid();

            var customer = new CustomerDto
            {
                CustomerId = customerId,
                BankAccounts = new List<Guid> { accountId },
                FirstName = "Jane",
                LastName = "Doe",
                CustomerAddress = "N/A",
                CustomerEmail = "unknown@example.com",
                CustomerPhone = "000-000-0000"
            };

            var account = new CurrentAccountDto
            {
                AccountId = accountId,
                Balance = 1000m,
                DateOpened = DateTime.UtcNow.AddDays(-10),
                Transactions = new List<TransactionDto>() // Empty list to simulate no transactions
            };

            _customerRepoMock.Setup(repo => repo.GetCustomerById(customerId))
                .ReturnsAsync(customer);

            _accountRepoMock.Setup(repo => repo.GetAccount(accountId))
                .ReturnsAsync(account);

            // Act
            var result = await _accountService.GetCustomerAccountDetails(customerId);

            // Assert
            Assert.NotNull(result);
        }
    }
}
