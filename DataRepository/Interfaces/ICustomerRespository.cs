using Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataRepository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerDto?> GetCustomerById(Guid id);
        Task<CustomerDto> CreateCustomer(CustomerDto customer);
        Task<CustomerDto?> UpdateCustomer(CustomerDto customer);
        Task<bool> DeleteCustomer(Guid id);
        Task<List<CustomerDto>> GetAllCustomers();
        Task AddAccountToCustomer(Guid customerId, Guid accountId);
    }
}