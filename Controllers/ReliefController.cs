using GiftOfTheGivers.Common;
using APPR_POE_FINAL.Data;
using APPR_POE_FINAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace APPR_POE_FINAL.Controllers
{
    public class ReliefController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReliefController(ApplicationDbContext context) { _context = context; }
        public async Task<IActionResult> Index()
        {
            var operations = await _context.ReliefOperations.OrderByDescending(r => r.DatePosted).ToListAsync();
            ViewBag.ReliefStatusMessage = ReliefHelper.GetReliefStatus("approved");
            return View(operations);
        }
        public IActionResult Create() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReliefOperation reliefOperation)
        {
            if (ModelState.IsValid)
            {
                reliefOperation.DatePosted = DateTime.Now;
                _context.ReliefOperations.Add(reliefOperation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reliefOperation);
        }
    }
}
