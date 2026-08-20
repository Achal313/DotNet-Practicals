using System.ComponentModel.DataAnnotations;

namespace VehicleRentalManagementSystem.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 10000000, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Payment method is required")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment status is required")]
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; } = "Pending";

        public RentalBooking? RentalBooking { get; set; }
    }
}