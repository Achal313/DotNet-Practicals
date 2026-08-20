using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Data;
using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments
                .Include(p => p.RentalBooking)
                    .ThenInclude(r => r.Customer)
                .Include(p => p.RentalBooking)
                    .ThenInclude(r => r.Vehicle)
                .OrderByDescending(p => p.PaymentId)
                .ToListAsync();

            return View(payments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var payment = await _context.Payments
                .Include(p => p.RentalBooking)
                    .ThenInclude(r => r.Customer)
                .Include(p => p.RentalBooking)
                    .ThenInclude(r => r.Vehicle)
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        public async Task<IActionResult> Create()
        {
            await LoadBookings();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            var booking = await _context.RentalBookings
                .FirstOrDefaultAsync(r => r.BookingId == payment.BookingId);

            if (booking == null)
            {
                ModelState.AddModelError(
                    "BookingId",
                    "Selected booking does not exist.");
            }
            else
            {
                if (booking.BookingStatus == "Cancelled")
                {
                    ModelState.AddModelError(
                        "BookingId",
                        "Payment cannot be made for a cancelled booking.");
                }

                bool paymentExists = await _context.Payments
                    .AnyAsync(p => p.BookingId == payment.BookingId);

                if (paymentExists)
                {
                    ModelState.AddModelError(
                        "BookingId",
                        "Payment already exists for this booking.");
                }

                if (payment.Amount != booking.TotalCharges)
                {
                    ModelState.AddModelError(
                        "Amount",
                        $"Payment amount must be ₹{booking.TotalCharges:N2}.");
                }
            }

            if (ModelState.IsValid)
            {
                payment.PaymentDate = DateTime.Now;
                payment.PaymentStatus = "Paid";

                _context.Payments.Add(payment);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Payment recorded successfully.";

                return RedirectToAction(nameof(Index));
            }

            await LoadBookings(payment.BookingId);

            return View(payment);
        }

        private async Task LoadBookings(int? selectedBookingId = null)
        {
            var bookings = await _context.RentalBookings
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .Where(r =>
                    r.BookingStatus != "Cancelled" &&
                    !_context.Payments.Any(p => p.BookingId == r.BookingId))
                .OrderByDescending(r => r.BookingId)
                .ToListAsync();

            ViewBag.BookingId = new SelectList(
                bookings,
                "BookingId",
                "BookingDisplay",
                selectedBookingId);
        }
    }
}