namespace APPR_POE_FINAL.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Volunteer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Skills { get; set; } = string.Empty;
        [Required]
        public string Availability { get; set; } = string.Empty;
    }
}
