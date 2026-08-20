using System.ComponentModel.DataAnnotations;

namespace VehicleRentalManagementSystem.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Vehicle number is required")]
        [Display(Name = "Vehicle Number")]
        public string VehicleNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vehicle name is required")]
        [Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vehicle type is required")]
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rent per day is required")]
        [Range(1, 100000, ErrorMessage = "Rent must be greater than 0")]
        [Display(Name = "Rent Per Day")]
        public decimal RentPerDay { get; set; }

        [Required(ErrorMessage = "Availability status is required")]
        [Display(Name = "Availability Status")]
        public string AvailabilityStatus { get; set; } = "Available";

        public ICollection<RentalBooking>? RentalBookings { get; set; }
    }
}