using VehicleRentalManagementSystem.Models;
using VehicleRentalManagementSystem.Repositories.Interfaces;
using VehicleRentalManagementSystem.Services.Interfaces;

namespace VehicleRentalManagementSystem.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _vehicleRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _vehicleRepository.GetAvailableVehiclesAsync();
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            bool exists = await _vehicleRepository
                .VehicleNumberExistsAsync(vehicle.VehicleNumber);

            if (exists)
                throw new InvalidOperationException(
                    "Vehicle number already exists.");

            await _vehicleRepository.AddAsync(vehicle);
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            if (!await _vehicleRepository.ExistsAsync(vehicle.VehicleId))
                throw new KeyNotFoundException("Vehicle not found.");

            bool exists = await _vehicleRepository
                .VehicleNumberExistsAsync(
                    vehicle.VehicleNumber,
                    vehicle.VehicleId);

            if (exists)
                throw new InvalidOperationException(
                    "Vehicle number already exists.");

            await _vehicleRepository.UpdateAsync(vehicle);
        }

        public async Task DeleteVehicleAsync(int id)
        {
            if (!await _vehicleRepository.ExistsAsync(id))
                throw new KeyNotFoundException("Vehicle not found.");

            await _vehicleRepository.DeleteAsync(id);
        }
    }
}
