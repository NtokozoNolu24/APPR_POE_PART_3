using APPR_POE_FINAL.Data;
using APPR_POE_FINAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APPR_POE_FINAL.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ViewBag.LoggedInName = User.Identity.Name;
            }
            var reviews = await _context.Reviews.OrderByDescending(r => r.DatePosted).ToListAsync();
            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Review review)
        {
            if (ModelState.IsValid)
            {
                // Automatically assign name if logged in
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    review.Name = User.Identity.Name;
                }

                review.DatePosted = DateTime.Now;
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thank you for your review!";
                return RedirectToAction(nameof(Index));
            }
            var reviews = await _context.Reviews.OrderByDescending(r => r.DatePosted).ToListAsync();
            return View(reviews);
        }
    }
}