using System.ComponentModel.DataAnnotations;

namespace VehicleRentalManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Driving license number is required")]
        [Display(Name = "Driving License Number")]
        public string DrivingLicenseNo { get; set; } = string.Empty;

        public ICollection<RentalBooking>? RentalBookings { get; set; }
    }
}