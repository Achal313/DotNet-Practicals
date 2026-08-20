namespace VehicleRentalManagementSystem.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(string username);
    }
}