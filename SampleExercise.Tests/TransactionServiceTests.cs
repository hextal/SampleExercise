using Moq;
using DataRepository.Interfaces;
using Microsoft.Extensions.Logging;
using Models.Dto;
using SampleExercise.Services;
using Xunit;
using Assert = Xunit.Assert;
namespace SampleExercise.Tests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ILogger<TransactionService>> _mockLogger;
        private readonly Mock<ITransactionRepository> _mockRepository;
        private readonly TransactionService _service;

        public TransactionServiceTests()
        {
            _mockLogger = new Mock<ILogger<TransactionService>>();
            _mockRepository = new Mock<ITransactionRepository>();
            _service = new TransactionService(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task CreateTransaction_ValidInput_ReturnsTransaction()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var transaction = new TransactionDto
            {
                TransactionId = Guid.NewGuid(),
                AccountId = accountId,
                TransactionType = "Deposit",
                Amount = 100m,
                TransactionDate = DateTime.UtcNow,
                TransactionStatus = "Pending",
                TransactionRemarks = "Initial deposit"
            };

            _mockRepository
                .Setup(repo => repo.CreateTransaction(accountId, It.IsAny<TransactionDto>()))
                .ReturnsAsync(transaction);

            // Act
            var result = await _service.CreateTransaction(accountId, "Deposit", 100m, "Initial deposit");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transaction.TransactionId, result.TransactionId);
            Assert.Equal(100m, result.Amount);
            _mockRepository.Verify(repo => repo.CreateTransaction(accountId, It.IsAny<TransactionDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateTransaction_InvalidAmount_ThrowsArgumentException()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateTransaction(accountId, "Deposit", -50m, "Invalid transaction"));

            _mockLogger.Verify(logger =>
                logger.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    null,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task GetTransaction_TransactionExists_ReturnsTransaction()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var transaction = new TransactionDto
            {
                TransactionId = transactionId,
                AccountId = Guid.NewGuid(),
                TransactionType = "Deposit",
                Amount = 200m,
                TransactionDate = DateTime.UtcNow,
                TransactionStatus = "Completed",
                TransactionRemarks = "Deposit"
            };

            _mockRepository
                .Setup(repo => repo.GetTransaction(transactionId))
                .ReturnsAsync(transaction);

            // Act
            var result = await _service.GetTransaction(transactionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transactionId, result.TransactionId);
            _mockRepository.Verify(repo => repo.GetTransaction(transactionId), Times.Once);
        }

        [Fact]
        public async Task GetTransaction_TransactionDoesNotExist_ReturnsNull()
        {
            // Arrange
            var transactionId = Guid.NewGuid();

            _mockRepository
                .Setup(repo => repo.GetTransaction(transactionId))
                .ReturnsAsync((TransactionDto?)null);

            // Act
            var result = await _service.GetTransaction(transactionId);

            // Assert
            Assert.Null(result);
            _mockLogger.Verify(logger =>
                logger.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    null,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteTransaction_TransactionExists_ReturnsTrue()
        {
            // Arrange
            var transactionId = Guid.NewGuid();

            _mockRepository
                .Setup(repo => repo.DeleteTransaction(transactionId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteTransaction(transactionId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.DeleteTransaction(transactionId), Times.Once);
        }

        [Fact]
        public async Task DeleteTransaction_TransactionDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var transactionId = Guid.NewGuid();

            _mockRepository
                .Setup(repo => repo.DeleteTransaction(transactionId))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteTransaction(transactionId);

            // Assert
            Assert.False(result);
            _mockLogger.Verify(logger =>
                logger.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    null,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateTransaction_ValidTransaction_ReturnsUpdatedTransaction()
        {
            // Arrange
            var transaction = new TransactionDto
            {
                TransactionId = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                TransactionType = "Withdrawal",
                Amount = 50m,
                TransactionDate = DateTime.UtcNow,
                TransactionStatus = "Completed",
                TransactionRemarks = "ATM withdrawal"
            };

            _mockRepository
                .Setup(repo => repo.UpdateTransaction(transaction))
                .ReturnsAsync(transaction);

            // Act
            var result = await _service.UpdateTransaction(transaction);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transaction.TransactionId, result?.TransactionId);
            _mockRepository.Verify(repo => repo.UpdateTransaction(transaction), Times.Once);
        }

        [Fact]
        public async Task UpdateTransaction_NonExistentTransaction_ReturnsNull()
        {
            // Arrange
            var transaction = new TransactionDto
            {
                TransactionId = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                TransactionType = "Deposit",
                Amount = 100m,
                TransactionDate = DateTime.UtcNow,
                TransactionStatus = "Pending",
                TransactionRemarks = "Initial deposit"
            };

            _mockRepository
                .Setup(repo => repo.UpdateTransaction(transaction))
                .ReturnsAsync((TransactionDto?)null);

            // Act
            var result = await _service.UpdateTransaction(transaction);

            // Assert
            Assert.Null(result);
            _mockLogger.Verify(logger =>
                logger.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    null,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task GetTransactionsByAccountId_AccountHasTransactions_ReturnsTransactions()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var transactions = new List<TransactionDto>
            {
                new TransactionDto
                {
                    TransactionId = Guid.NewGuid(),
                    AccountId = accountId,
                    TransactionType = "Deposit",
                    Amount = 100m,
                    TransactionDate = DateTime.UtcNow,
                    TransactionStatus = "Completed",
                    TransactionRemarks = "Monthly savings"
                },
                new TransactionDto
                {
                    TransactionId = Guid.NewGuid(),
                    AccountId = accountId,
                    TransactionType = "Withdrawal",
                    Amount = 50m,
                    TransactionDate = DateTime.UtcNow.AddDays(-1),
                    TransactionStatus = "Pending",
                    TransactionRemarks = "ATM withdrawal"
                }
            };

            _mockRepository
                .Setup(repo => repo.GetTransactionsByAccountId(accountId))
                .ReturnsAsync(transactions);

            // Act
            var result = await _service.GetTransactionsByAccountId(accountId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.NotNull(t.TransactionType)); // Ensures TransactionType is set
            Assert.All(result, t => Assert.NotNull(t.TransactionStatus)); // Ensures TransactionStatus is set
            _mockRepository.Verify(repo => repo.GetTransactionsByAccountId(accountId), Times.Once);
        }
    }
}
