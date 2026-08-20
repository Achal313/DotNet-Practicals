using System.ComponentModel.DataAnnotations;

namespace VehicleRentalManagementSystem.Models
{
    public class RentalBooking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Pickup date is required")]
        [Display(Name = "Pickup Date")]
        public DateTime PickupDate { get; set; }

        [Required(ErrorMessage = "Return date is required")]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Total Days")]
        public int TotalDays { get; set; }
        [Range(0, 10000000)]
        [Display(Name = "Total Charges")]
        public decimal TotalCharges { get; set; }

        [Required]
        [Display(Name = "Booking Status")]
        public string BookingStatus { get; set; } = "Pending";

        public Customer? Customer { get; set; }

        public Vehicle? Vehicle { get; set; }

        public Payment? Payment { get; set; }

        public string BookingDisplay =>
            $"#{BookingId} - {Customer?.Name} - {Vehicle?.VehicleName} - ₹{TotalCharges:N2}";
    }
}