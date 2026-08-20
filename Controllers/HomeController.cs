using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Data;

namespace VehicleRentalManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalVehicles = await _context.Vehicles.CountAsync();
            ViewBag.AvailableVehicles = await _context.Vehicles
                .CountAsync(v => v.AvailabilityStatus == "Available");

            ViewBag.TotalCustomers = await _context.Customers.CountAsync();
            ViewBag.TotalBookings = await _context.RentalBookings.CountAsync();

            ViewBag.TotalRevenue = await _context.RentalBookings
                .Where(r => r.BookingStatus == "Completed")
                .SumAsync(r => (decimal?)r.TotalCharges) ?? 0;

            return View();
        }
    }
}