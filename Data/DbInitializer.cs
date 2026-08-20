using Microsoft.EntityFrameworkCore;
using VehicleRentalManagementSystem.Models;

namespace VehicleRentalManagementSystem.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            if (!await context.Vehicles.AnyAsync())
            {
                context.Vehicles.AddRange(
                    new Vehicle
                    {
                        VehicleNumber = "MH12AB1234",
                        VehicleName = "Swift",
                        VehicleType = "Car",
                        Brand = "Maruti Suzuki",
                        Model = "2025",
                        RentPerDay = 1500,
                        AvailabilityStatus = "Available"
                    },

                    new Vehicle
                    {
                        VehicleNumber = "MH12CD5678",
                        VehicleName = "Activa 6G",
                        VehicleType = "Bike",
                        Brand = "Honda",
                        Model = "2025",
                        RentPerDay = 500,
                        AvailabilityStatus = "Available"
                    },

                    new Vehicle
                    {
                        VehicleNumber = "MH14EF9012",
                        VehicleName = "Creta",
                        VehicleType = "SUV",
                        Brand = "Hyundai",
                        Model = "2024",
                        RentPerDay = 2500,
                        AvailabilityStatus = "Available"
                    },

                    new Vehicle
                    {
                        VehicleNumber = "MH12GH3456",
                        VehicleName = "Innova Crysta",
                        VehicleType = "SUV",
                        Brand = "Toyota",
                        Model = "2024",
                        RentPerDay = 3000,
                        AvailabilityStatus = "Available"
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!await context.Customers.AnyAsync())
            {
                context.Customers.AddRange(
                    new Customer
                    {
                        Name = "Rahul Patil",
                        Email = "rahul@gmail.com",
                        Phone = "9876543210",
                        Address = "Pune, Maharashtra",
                        DrivingLicenseNo = "MH12-2025-123456"
                    },

                    new Customer
                    {
                        Name = "Priya Sharma",
                        Email = "priya@gmail.com",
                        Phone = "9876543211",
                        Address = "Pune, Maharashtra",
                        DrivingLicenseNo = "MH12-2025-654321"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}