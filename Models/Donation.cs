using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPR_POE_FINAL.Models
{
    public class Donation
    {
        public int Id { get; set; }

        // NEW: Tracks the name of the donor
        public string? DonorName { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a valid donation amount.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string DonationType { get; set; } = string.Empty;

        [Required]
        public string Currency { get; set; } = string.Empty;

        public bool IsAnonymous { get; set; }

        public DateTime DonationDate { get; set; } = DateTime.Now;
    }
}