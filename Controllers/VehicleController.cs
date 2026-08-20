using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Data;
using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicles
                .OrderByDescending(v => v.VehicleId)
                .ToListAsync();

            return View(vehicles);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == id);

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.AvailabilityStatus = "Available";

                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Vehicle added successfully.";

                return RedirectToAction(nameof(Index));
            }

            return View(vehicle);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Vehicle updated successfully.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.VehicleId))
                        return NotFound();

                    throw;
                }
            }

            return View(vehicle);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == id);

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return NotFound();

            bool hasBookings = await _context.RentalBookings
                .AnyAsync(r => r.VehicleId == id);

            if (hasBookings)
            {
                TempData["Error"] =
                    "This vehicle cannot be deleted because it has rental bookings.";

                return RedirectToAction(nameof(Index));
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Vehicle deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }
    }
}