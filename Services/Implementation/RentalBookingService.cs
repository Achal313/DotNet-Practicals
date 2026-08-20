using VehicleRentalManagementSystem.Models;
using VehicleRentalManagementSystem.Repositories.Interfaces;
using VehicleRentalManagementSystem.Services.Interfaces;

namespace VehicleRentalManagementSystem.Services.Implementations
{
    public class RentalBookingService : IRentalBookingService
    {
        private readonly IRentalBookingRepository _bookingRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ICustomerRepository _customerRepository;

        public RentalBookingService(
            IRentalBookingRepository bookingRepository,
            IVehicleRepository vehicleRepository,
            ICustomerRepository customerRepository)
        {
            _bookingRepository = bookingRepository;
            _vehicleRepository = vehicleRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<RentalBooking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<RentalBooking?> GetBookingByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RentalBooking>> GetActiveBookingsAsync()
        {
            return await _bookingRepository.GetActiveBookingsAsync();
        }

        public async Task AddBookingAsync(RentalBooking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            if (booking.CustomerId <= 0)
                throw new InvalidOperationException(
                    "Please select a customer.");

            if (booking.VehicleId <= 0)
                throw new InvalidOperationException(
                    "Please select a vehicle.");

            if (booking.PickupDate.Date < DateTime.Today)
                throw new InvalidOperationException(
                    "Pickup date cannot be in the past.");

            if (booking.ReturnDate.Date <= booking.PickupDate.Date)
                throw new InvalidOperationException(
                    "Return date must be after pickup date.");

            var customer = await _customerRepository
                .GetByIdAsync(booking.CustomerId);

            if (customer == null)
                throw new KeyNotFoundException(
                    "Selected customer was not found.");

            var vehicle = await _vehicleRepository
                .GetByIdAsync(booking.VehicleId);

            if (vehicle == null)
                throw new KeyNotFoundException(
                    "Selected vehicle was not found.");

            if (vehicle.AvailabilityStatus != "Available")
                throw new InvalidOperationException(
                    "Selected vehicle is not available.");

            // Check overlapping bookings
            var activeBookings =
                await _bookingRepository.GetActiveBookingsAsync();

            bool overlappingBooking = activeBookings.Any(r =>
                r.VehicleId == booking.VehicleId &&
                booking.PickupDate.Date < r.ReturnDate.Date &&
                booking.ReturnDate.Date > r.PickupDate.Date);

            if (overlappingBooking)
                throw new InvalidOperationException(
                    "Vehicle is already booked for the selected dates.");

            // Calculate rental days
            booking.TotalDays =
                (booking.ReturnDate.Date -
                 booking.PickupDate.Date).Days;

            if (booking.TotalDays <= 0)
                throw new InvalidOperationException(
                    "Rental duration must be at least 1 day.");

            // Calculate charges
            booking.TotalCharges =
                booking.TotalDays * vehicle.RentPerDay;

            // Set booking status
            booking.BookingStatus = "Confirmed";

            // Save booking
            await _bookingRepository.AddAsync(booking);

            // Change vehicle status
            vehicle.AvailabilityStatus = "Rented";

            await _vehicleRepository.UpdateAsync(vehicle);
        }

        public async Task UpdateBookingAsync(RentalBooking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            if (!await _bookingRepository.ExistsAsync(
                    booking.BookingId))
            {
                throw new KeyNotFoundException(
                    "Booking not found.");
            }

            if (booking.PickupDate.Date < DateTime.Today)
                throw new InvalidOperationException(
                    "Pickup date cannot be in the past.");

            if (booking.ReturnDate.Date <= booking.PickupDate.Date)
                throw new InvalidOperationException(
                    "Return date must be after pickup date.");

            var vehicle = await _vehicleRepository
                .GetByIdAsync(booking.VehicleId);

            if (vehicle == null)
                throw new KeyNotFoundException(
                    "Vehicle not found.");

            booking.TotalDays =
                (booking.ReturnDate.Date -
                 booking.PickupDate.Date).Days;

            booking.TotalCharges =
                booking.TotalDays * vehicle.RentPerDay;

            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                throw new KeyNotFoundException(
                    "Booking not found.");

            var vehicle = await _vehicleRepository
                .GetByIdAsync(booking.VehicleId);

            await _bookingRepository.DeleteAsync(id);

            if (vehicle != null)
            {
                vehicle.AvailabilityStatus = "Available";

                await _vehicleRepository.UpdateAsync(vehicle);
            }
        }
    }
}