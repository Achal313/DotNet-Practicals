using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();

        Task<Payment?> GetPaymentByIdAsync(int id);

        Task<Payment?> GetPaymentByBookingIdAsync(int bookingId);

        Task AddPaymentAsync(Payment payment);

        Task UpdatePaymentAsync(Payment payment);

        Task DeletePaymentAsync(int id);
    }
}