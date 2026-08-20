using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Data;
using VehicleRentalManagementSystem.Models;
using VehicleRentalManagementSystem.Repositories.Interfaces;

namespace VehicleRentalManagementSystem.Repositories.Implementations
{
    public class RentalBookingRepository : IRentalBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public RentalBookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RentalBooking>> GetAllAsync()
        {
            return await _context.RentalBookings
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .OrderByDescending(r => r.BookingId)
                .ToListAsync();
        }

        public async Task<RentalBooking?> GetByIdAsync(int id)
        {
            return await _context.RentalBookings
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.BookingId == id);
        }

        public async Task<IEnumerable<RentalBooking>> GetActiveBookingsAsync()
        {
            return await _context.RentalBookings
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .Where(r =>
                    r.BookingStatus != "Cancelled" &&
                    r.BookingStatus != "Completed")
                .OrderByDescending(r => r.BookingId)
                .ToListAsync();
        }

        public async Task AddAsync(RentalBooking booking)
        {
            await _context.RentalBookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RentalBooking booking)
        {
            _context.RentalBookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var booking = await GetByIdAsync(id);

            if (booking != null)
            {
                _context.RentalBookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RentalBookings
                .AnyAsync(r => r.BookingId == id);
        }
    }
}