namespace SampleExercise.Models;

public class CurrentAccount
{
    private decimal _balance;
    private DateTime _dateOpened;
    private Guid _accountId;

    public CurrentAccount(decimal balance, DateTime dateOpened, Guid accountId)
    {
        _balance = balance;
        _dateOpened = dateOpened;
        _accountId = accountId;
    }
    
}