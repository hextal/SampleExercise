public class TransactionDto
{
    public Guid TransactionId { get; set; }
    public Guid AccountId { get; set; }
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionStatus { get; set; }
    public string TransactionRemarks { get; set; }

    public TransactionDto(Guid transactionId, Guid accountId, string transactionType, decimal amount, DateTime transactionDate, string transactionStatus, string transactionRemarks)
    {
        TransactionId = transactionId;
        AccountId = accountId;
        TransactionType = transactionType;
        Amount = amount;
        TransactionDate = transactionDate;
        TransactionStatus = transactionStatus;
        TransactionRemarks = transactionRemarks;
    }
}