using APPR_POE_FINAL.Data;
using APPR_POE_FINAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace APPR_POE_FINAL.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ViewBag.LoggedInName = User.Identity.Name;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                // Automatically assign name if logged in
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    volunteer.Name = User.Identity.Name;
                }

                _context.Volunteers.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThankYou));
            }
            return View(volunteer);
        }

        public IActionResult ThankYou() => View();
    }
}