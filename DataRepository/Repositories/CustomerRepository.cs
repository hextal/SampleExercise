using DataRepository.Interfaces;

namespace DataRepository.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private static List<CustomerDto> _customers = new List<CustomerDto>();

        public async Task<CustomerDto?> GetCustomerById(Guid id)
        {
            var customer = _customers.FirstOrDefault(c => c.CustomerId == id);
            return await Task.FromResult(customer);
        }

        public async Task<CustomerDto> CreateCustomer(CustomerDto customer)
        {
            customer.CustomerId = Guid.NewGuid();
            _customers.Add(customer);
            return await Task.FromResult(customer);
        }

        public async Task<CustomerDto?> UpdateCustomer(CustomerDto customer)
        {
            var existingCustomer = _customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (existingCustomer == null)
            {
                return await Task.FromResult<CustomerDto?>(null);
            }

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.CustomerAddress = customer.CustomerAddress;
            existingCustomer.CustomerEmail = customer.CustomerEmail;
            existingCustomer.CustomerPhone = customer.CustomerPhone;
            existingCustomer.DateUpdated = DateTime.UtcNow;
            existingCustomer.BankAccounts = customer.BankAccounts;

            return await Task.FromResult(existingCustomer);
        }

        public async Task<bool> DeleteCustomer(Guid id)
        {
            var customer = _customers.FirstOrDefault(c => c.CustomerId == id);
            if (customer == null)
            {
                return await Task.FromResult(false);
            }

            _customers.Remove(customer);
            return await Task.FromResult(true);
        }

        public async Task<List<CustomerDto>> GetAllCustomers()
        {
            return await Task.FromResult(_customers);
        }

        public async Task AddAccountToCustomer(Guid customerId, Guid accountId)
        {
            var customer = _customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer != null)
            {
                customer.BankAccounts.Add(accountId);
                await Task.CompletedTask;
            }
        }
    }
}
