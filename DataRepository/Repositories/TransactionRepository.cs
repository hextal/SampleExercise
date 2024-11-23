using DataRepository.Interfaces;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataRepository.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private static List<TransactionDto> _transactions = new List<TransactionDto>();
        private static List<CurrentAccountDto> _accounts = new List<CurrentAccountDto>();

        public Task<TransactionDto?> GetTransaction(Guid transactionId)
        {
            var transaction = _transactions.FirstOrDefault(t => t.TransactionId == transactionId);
            return Task.FromResult(transaction);
        }

        public async Task<TransactionDto> CreateTransaction(Guid accountId, TransactionDto transaction)
        {
            transaction.TransactionId = Guid.NewGuid();
            var account = _accounts.FirstOrDefault(a => a.AccountId == accountId);

            if (account == null)
            {
                throw new InvalidOperationException("Account not found.");
            }

            account.Transactions.Add(transaction);
            _transactions.Add(transaction);
            
            account.Balance += transaction.TransactionType == "Deposit" ? transaction.Amount : -transaction.Amount;

            return await Task.FromResult(transaction);
        }

        public Task<TransactionDto?> UpdateTransaction(TransactionDto transaction)
        {
            var existingTransaction = _transactions.FirstOrDefault(t => t.TransactionId == transaction.TransactionId);
            if (existingTransaction == null)
            {
                return Task.FromResult<TransactionDto?>(null);
            }

            existingTransaction.TransactionType = transaction.TransactionType;
            existingTransaction.Amount = transaction.Amount;
            existingTransaction.TransactionDate = transaction.TransactionDate;
            existingTransaction.TransactionStatus = transaction.TransactionStatus;
            existingTransaction.TransactionRemarks = transaction.TransactionRemarks;

            return Task.FromResult(existingTransaction);
        }

        public Task<bool> DeleteTransaction(Guid transactionId)
        {
            var transaction = _transactions.FirstOrDefault(t => t.TransactionId == transactionId);
            if (transaction == null)
            {
                return Task.FromResult(false);
            }

            var account = _accounts.FirstOrDefault(a => a.AccountId == transaction.AccountId);
            if (account != null)
            {
                account.Transactions.Remove(transaction);
                account.Balance -= transaction.TransactionType == "Deposit" ? transaction.Amount : -transaction.Amount;
            }

            _transactions.Remove(transaction);
            return Task.FromResult(true);
        }

        public Task<List<TransactionDto>> GetTransactionsByAccountId(Guid accountId)
        {
            var accountTransactions = _transactions.Where(t => t.AccountId == accountId).ToList();
            return Task.FromResult(accountTransactions);
        }
    }
}
