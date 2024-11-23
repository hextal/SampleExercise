using Models.Dto;

public class CustomerDto
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CustomerAddress { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public List<Guid> BankAccounts { get; set; } // This should just contain GUIDs of bank accounts.
    
    public CustomerDto(Guid customerId, string firstName, string lastName, string customerAddress, string customerEmail, string customerPhone, DateTime dateCreated, DateTime dateUpdated, List<Guid> bankAccounts)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        CustomerAddress = customerAddress;
        CustomerEmail = customerEmail;
        CustomerPhone = customerPhone;
        DateCreated = dateCreated;
        DateUpdated = dateUpdated;
        BankAccounts = bankAccounts ?? new List<Guid>(); // Ensure it's not null
    }
}
