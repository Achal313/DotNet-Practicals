using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRentalManagementSystem.Models;
using VehicleRentalManagementSystem.Services.Interfaces;

namespace VehicleRentalManagementSystem.Controllers
{
    public class RentalBookingController : Controller
    {
        private readonly IRentalBookingService _bookingService;
        private readonly ICustomerService _customerService;
        private readonly IVehicleService _vehicleService;

        public RentalBookingController(
            IRentalBookingService bookingService,
            ICustomerService customerService,
            IVehicleService vehicleService)
        {
            _bookingService = bookingService;
            _customerService = customerService;
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();

            return View(bookings);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var booking =
                await _bookingService.GetBookingByIdAsync(id.Value);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalBooking booking)
        {
            // Customer validation
            if (booking.CustomerId <= 0)
            {
                ModelState.AddModelError(
                    "CustomerId",
                    "Please select a customer.");
            }

            // Vehicle validation
            if (booking.VehicleId <= 0)
            {
                ModelState.AddModelError(
                    "VehicleId",
                    "Please select a vehicle.");
            }

            // Pickup date validation
            if (booking.PickupDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(
                    "PickupDate",
                    "Pickup date cannot be in the past.");
            }

            // Return date validation
            if (booking.ReturnDate.Date <= booking.PickupDate.Date)
            {
                ModelState.AddModelError(
                    "ReturnDate",
                    "Return date must be after pickup date.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdowns(
                    booking.CustomerId,
                    booking.VehicleId);

                return View(booking);
            }

            try
            {
                await _bookingService.AddBookingAsync(booking);

                TempData["Success"] =
                    "Rental booking created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Booking could not be created: " +
                    ex.Message);
            }

            await LoadDropdowns(
                booking.CustomerId,
                booking.VehicleId);

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var booking =
                    await _bookingService.GetBookingByIdAsync(id);

                if (booking == null)
                    return NotFound();

                if (booking.BookingStatus == "Completed" ||
                    booking.BookingStatus == "Cancelled")
                {
                    TempData["Error"] =
                        "This booking cannot be cancelled.";

                    return RedirectToAction(nameof(Index));
                }

                booking.BookingStatus = "Cancelled";

                await _bookingService.UpdateBookingAsync(booking);

                TempData["Success"] =
                    "Booking cancelled successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnVehicle(int id)
        {
            try
            {
                var booking =
                    await _bookingService.GetBookingByIdAsync(id);

                if (booking == null)
                    return NotFound();

                if (booking.BookingStatus == "Cancelled" ||
                    booking.BookingStatus == "Completed")
                {
                    TempData["Error"] =
                        "Vehicle cannot be returned for this booking.";

                    return RedirectToAction(nameof(Index));
                }

                booking.BookingStatus = "Completed";

                await _bookingService.UpdateBookingAsync(booking);

                TempData["Success"] =
                    "Vehicle returned successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }

        private async Task LoadDropdowns(
            int? selectedCustomerId = null,
            int? selectedVehicleId = null)
        {
            var customers =
                await _customerService.GetAllCustomersAsync();

            var vehicles =
                await _vehicleService.GetAvailableVehiclesAsync();

            ViewBag.CustomerId = new SelectList(
                customers,
                "CustomerId",
                "Name",
                selectedCustomerId);

            ViewBag.VehicleId = new SelectList(
                vehicles,
                "VehicleId",
                "VehicleName",
                selectedVehicleId);
        }
    }
}