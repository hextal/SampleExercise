

namespace SampleExercise.Services;

public class TransactionService
{
    private readonly ILogger<TransactionService> _logger;
    
    public TransactionService(ILogger<TransactionService> logger)
    {
        _logger = logger;
    }
    
    private void CreateTransaction()
    {
        _logger.LogInformation("Transaction created");
    }
    
}