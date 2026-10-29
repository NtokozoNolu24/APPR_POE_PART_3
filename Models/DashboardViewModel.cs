namespace APPR_POE_FINAL.Models
{
    public class DashboardViewModel
    {
        public int TotalVolunteers { get; set; }
        public int TotalDonations { get; set; }
        public decimal TotalMoneyRaised { get; set; }
        public List<Volunteer> Volunteers { get; set; } = new();
        public List<Donation> RecentDonations { get; set; } = new();
    }
}
