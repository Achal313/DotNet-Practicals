using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        Task<Customer?> GetCustomerByIdAsync(int id);

        Task AddCustomerAsync(Customer customer);

        Task UpdateCustomerAsync(Customer customer);

        Task DeleteCustomerAsync(int id);
    }
}