using Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataRepository.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionDto?> GetTransaction(Guid transactionId);
        Task<TransactionDto> CreateTransaction(Guid accountId, TransactionDto transaction);
        Task<TransactionDto?> UpdateTransaction(TransactionDto transaction);
        Task<bool> DeleteTransaction(Guid transactionId);
        Task<List<TransactionDto>> GetTransactionsByAccountId(Guid accountId);
    }
}