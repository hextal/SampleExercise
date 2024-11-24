using DataRepository.Interfaces;
using Models.Dto;
using SampleExercise.Interfaces;

namespace SampleExercise.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ILogger<TransactionService> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionDto> CreateTransaction(Guid accountId, string transactionType, decimal amount, string remarks)
        {
            _logger.LogInformation("Attempting to create transaction for Account ID: {AccountId}", accountId);

            if (amount <= 0)
            {
                _logger.LogWarning("Invalid transaction amount: {Amount}", amount);
                throw new ArgumentException("Transaction amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(transactionType))
            {
                _logger.LogWarning("Transaction type is empty or null.");
                throw new ArgumentException("Transaction type must be provided.");
            }

            var transaction = new TransactionDto
            {
                TransactionType = transactionType,
                Amount = amount,
                TransactionDate = DateTime.UtcNow,
                TransactionRemarks = remarks,
                TransactionStatus = "Pending"
            };
            
            var createdTransaction = await _transactionRepository.CreateTransaction(accountId, transaction);

            _logger.LogInformation("Transaction successfully created with ID: {TransactionId}", createdTransaction.TransactionId);

            return createdTransaction;
        }

        public async Task<TransactionDto?> GetTransaction(Guid transactionId)
        {
            _logger.LogInformation("Fetching transaction with ID: {TransactionId}", transactionId);

            var transaction = await _transactionRepository.GetTransaction(transactionId);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction with ID: {TransactionId} not found.", transactionId);
            }

            return transaction;
        }

        public async Task<bool> DeleteTransaction(Guid transactionId)
        {
            _logger.LogInformation("Attempting to delete transaction with ID: {TransactionId}", transactionId);

            var result = await _transactionRepository.DeleteTransaction(transactionId);

            if (result)
            {
                _logger.LogInformation("Transaction with ID: {TransactionId} successfully deleted.", transactionId);
            }
            else
            {
                _logger.LogWarning("Transaction with ID: {TransactionId} not found or could not be deleted.", transactionId);
            }

            return result;
        }

        public async Task<TransactionDto?> UpdateTransaction(TransactionDto transaction)
        {
            _logger.LogInformation("Attempting to update transaction with ID: {TransactionId}", transaction.TransactionId);

            var updatedTransaction = await _transactionRepository.UpdateTransaction(transaction);

            if (updatedTransaction != null)
            {
                _logger.LogInformation("Transaction with ID: {TransactionId} successfully updated.", transaction.TransactionId);
            }
            else
            {
                _logger.LogWarning("Transaction with ID: {TransactionId} not found or could not be updated.", transaction.TransactionId);
            }

            return updatedTransaction;
        }

        public async Task<List<TransactionDto>> GetTransactionsByAccountId(Guid accountId)
        {
            _logger.LogInformation("Fetching all transactions for Account ID: {AccountId}", accountId);

            var transactions = await _transactionRepository.GetTransactionsByAccountId(accountId);

            if (!transactions.Any())
            {
                _logger.LogWarning("No transactions found for Account ID: {AccountId}", accountId);
            }

            return transactions;
        }
    }
}
