using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();

        Task<Payment?> GetByIdAsync(int id);

        Task<Payment?> GetByBookingIdAsync(int bookingId);

        Task AddAsync(Payment payment);

        Task UpdateAsync(Payment payment);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> PaymentExistsForBookingAsync(int bookingId);
    }
}