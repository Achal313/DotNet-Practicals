using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Repositories.Interfaces
{
    public interface IRentalBookingRepository
    {
        Task<IEnumerable<RentalBooking>> GetAllAsync();

        Task<RentalBooking?> GetByIdAsync(int id);

        Task<IEnumerable<RentalBooking>> GetActiveBookingsAsync();

        Task AddAsync(RentalBooking booking);

        Task UpdateAsync(RentalBooking booking);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}