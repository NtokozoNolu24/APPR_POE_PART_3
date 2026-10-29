namespace APPR_POE_FINAL.Models
{
    using System.ComponentModel.DataAnnotations;
    public class ReliefOperation
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        [Required]
        public string Update { get; set; } = string.Empty;
        public DateTime DatePosted { get; set; } = DateTime.Now;
    }
}
