namespace SampleExercise.Models;

public class TransactionDto
{
    private Guid _transactionId;
    private Guid _accountId;
    private string _transactionType;
    private decimal _amount;
    private DateTime _transactionDate;    
    private string _transactionStatus;
    private string _transactionRemarks;

    public TransactionDto(Guid transactionId, Guid accountId, string transactionType, decimal amount, DateTime transactionDate, string transactionStatus, string transactionRemarks)
    {
        _transactionId = transactionId;
        _accountId = accountId;
        _transactionType = transactionType;
        _amount = amount;
        _transactionDate = transactionDate;
        _transactionStatus = transactionStatus;
        _transactionRemarks = transactionRemarks;
    }
}