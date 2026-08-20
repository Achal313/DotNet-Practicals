using VehicleRentalManagementSystem.Models;
using VehicleRentalManagementSystem.Repositories.Interfaces;
using VehicleRentalManagementSystem.Services.Interfaces;

namespace VehicleRentalManagementSystem.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IRentalBookingRepository _bookingRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IRentalBookingRepository bookingRepository)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task<Payment?> GetPaymentByBookingIdAsync(int bookingId)
        {
            if (bookingId <= 0)
                return null;

            return await _paymentRepository
                .GetByBookingIdAsync(bookingId);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            var booking = await _bookingRepository
                .GetByIdAsync(payment.BookingId);

            if (booking == null)
                throw new KeyNotFoundException(
                    "Rental booking not found.");

            bool paymentExists =
                await _paymentRepository
                    .PaymentExistsForBookingAsync(payment.BookingId);

            if (paymentExists)
            {
                throw new InvalidOperationException(
                    "Payment already exists for this booking.");
            }

            if (payment.Amount != booking.TotalCharges)
            {
                throw new InvalidOperationException(
                    "Payment amount must match total booking charges.");
            }

            payment.PaymentDate = DateTime.Now;
            payment.PaymentStatus = "Paid";

            await _paymentRepository.AddAsync(payment);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            if (!await _paymentRepository.ExistsAsync(payment.PaymentId))
                throw new KeyNotFoundException("Payment not found.");

            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task DeletePaymentAsync(int id)
        {
            if (!await _paymentRepository.ExistsAsync(id))
                throw new KeyNotFoundException("Payment not found.");

            await _paymentRepository.DeleteAsync(id);
        }
    }
}