using APPR_POE_FINAL.Data;
using APPR_POE_FINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace APPR_POE_FINAL.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context) { _context = context; }
        public async Task<IActionResult> Dashboard()
        {
            var model = new DashboardViewModel
            {
                TotalVolunteers = await _context.Volunteers.CountAsync(),
                TotalDonations = await _context.Donations.CountAsync(),
                TotalMoneyRaised = await _context.Donations.SumAsync(d => d.Amount),
                Volunteers = await _context.Volunteers.OrderByDescending(v => v.Id).ToListAsync(),
                RecentDonations = await _context.Donations.OrderByDescending(d => d.DonationDate).Take(10).ToListAsync()
            };
            return View(model);
        }
    }
}
