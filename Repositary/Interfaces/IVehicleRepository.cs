using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();

        Task<Vehicle?> GetByIdAsync(int id);

        Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync();

        Task AddAsync(Vehicle vehicle);

        Task UpdateAsync(Vehicle vehicle);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> VehicleNumberExistsAsync(
            string vehicleNumber,
            int? excludeVehicleId = null);
    }
}