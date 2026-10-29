using System.ComponentModel.DataAnnotations;

namespace APPR_POE_FINAL.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Please select a rating between 1 and 5.")]
        public int Rating { get; set; }

        [Required]
        [Display(Name = "Review Comment")]
        public string Comment { get; set; } = string.Empty;

        public DateTime DatePosted { get; set; } = DateTime.Now;
    }
}