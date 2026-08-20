using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();

        Task<Vehicle?> GetVehicleByIdAsync(int id);

        Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync();

        Task AddVehicleAsync(Vehicle vehicle);

        Task UpdateVehicleAsync(Vehicle vehicle);

        Task DeleteVehicleAsync(int id);
    }
}
