using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Data;
using VehicleRentalManagementSystem.Models;
using VehicleRentalManagementSystem.Repositories.Interfaces;

namespace VehicleRentalManagementSystem.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.RentalBooking)
                    .ThenInclude(r => r.Customer)
                .Include(p => p.RentalBooking)
                    .ThenInclude(r => r.Vehicle)
                .OrderByDescending(p => p.PaymentId)
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.RentalBooking)
                    .ThenInclude(r => r.Customer)
                .Include(p => p.RentalBooking)
                    .ThenInclude(r => r.Vehicle)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<Payment?> GetByBookingIdAsync(int bookingId)
        {
            return await _context.Payments
                .Include(p => p.RentalBooking)
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await GetByIdAsync(id);

            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Payments
                .AnyAsync(p => p.PaymentId == id);
        }

        public async Task<bool> PaymentExistsForBookingAsync(int bookingId)
        {
            return await _context.Payments
                .AnyAsync(p => p.BookingId == bookingId);
        }
    }
}