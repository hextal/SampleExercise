using System.Transactions;

namespace SampleExercise.Models;

public class CustomerDto
{
    private Guid _customerId;
    private string _firstName;
    private string _lastName;
    private string _customerAddress;
    private string _customerEmail;
    private string _customerContactNumber;
    private CurrentAccount _currentAccount;
    private List<TransactionDto> _transactions;
    
    public CustomerDto(Guid customerId, string firstName, string lastName, string customerAddress, string customerEmail, string customerContactNumber, CurrentAccount currentAccount, List<TransactionDto> transactions)
    {
        _customerId = customerId;
        _firstName = firstName;
        _lastName = lastName;
        _customerAddress = customerAddress;
        _customerEmail = customerEmail;
        _customerContactNumber = customerContactNumber;
        _currentAccount = currentAccount;
        _transactions = transactions;
    }
}