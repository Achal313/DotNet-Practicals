using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Data;
using VehicleRentalManagementSystem.Models;
using VehicleRentalManagementSystem.Repositories.Interfaces;

namespace VehicleRentalManagementSystem.Repositories.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                .OrderByDescending(v => v.VehicleId)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == id);
        }

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _context.Vehicles
                .Where(v => v.AvailabilityStatus == "Available")
                .OrderBy(v => v.VehicleName)
                .ToListAsync();
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vehicle = await GetByIdAsync(id);

            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Vehicles
                .AnyAsync(v => v.VehicleId == id);
        }

        public async Task<bool> VehicleNumberExistsAsync(
            string vehicleNumber,
            int? excludeVehicleId = null)
        {
            return await _context.Vehicles.AnyAsync(v =>
                v.VehicleNumber == vehicleNumber &&
                (!excludeVehicleId.HasValue ||
                 v.VehicleId != excludeVehicleId.Value));
        }
    }
}