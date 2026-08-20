using VehicleRentalManagementSystem.Models;
using VehicleRentalManagementSystem.Repositories.Interfaces;
using VehicleRentalManagementSystem.Services.Interfaces;

namespace VehicleRentalManagementSystem.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            await _customerRepository.AddAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (!await _customerRepository.ExistsAsync(customer.CustomerId))
                throw new KeyNotFoundException("Customer not found.");

            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            if (!await _customerRepository.ExistsAsync(id))
                throw new KeyNotFoundException("Customer not found.");

            await _customerRepository.DeleteAsync(id);
        }
    }
}