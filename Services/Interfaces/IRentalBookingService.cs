using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Services.Interfaces
{
    public interface IRentalBookingService
    {
        Task<IEnumerable<RentalBooking>> GetAllBookingsAsync();

        Task<RentalBooking?> GetBookingByIdAsync(int id);

        Task<IEnumerable<RentalBooking>> GetActiveBookingsAsync();

        Task AddBookingAsync(RentalBooking booking);

        Task UpdateBookingAsync(RentalBooking booking);

        Task DeleteBookingAsync(int id);
    }
}