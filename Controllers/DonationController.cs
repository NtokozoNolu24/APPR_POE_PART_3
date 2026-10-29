using APPR_POE_FINAL.Data;
using APPR_POE_FINAL.Models;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace APPR_POE_FINAL.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationController(ApplicationDbContext context)
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
        public async Task<IActionResult> Index(Donation donation)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    donation.DonorName = User.Identity.Name;
                }

                donation.DonationDate = DateTime.Now;
                _context.Donations.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TaxCertificate), new { id = donation.Id });
            }
            return View(donation);
        }

        public async Task<IActionResult> TaxCertificate(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null) return NotFound();
            return View(donation);
        }

        // PDF GENERATOR
        public async Task<IActionResult> DownloadCertificate(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null) return NotFound();

            // Disable font glyph checking to prevent Azure crashes
            QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    // Use the default safe font
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header()
                        .Text("Gift of the Givers Foundation - Tax Certificate")
                        .SemiBold().FontSize(20).FontColor(Colors.Green.Darken2);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text($"Date Issued: {DateTime.Now:dd MMMM yyyy}");
                            x.Item().Text($"Certificate Reference: GOTG-{donation.Id:D5}");
                            x.Item().LineHorizontal(1);

                            x.Item().Text(text =>
                            {
                                text.Span("Donor Name: ").SemiBold();
                                text.Span(donation.IsAnonymous ? "Anonymous Donor" : (donation.DonorName ?? "Guest"));
                            });

                            x.Item().Text(text =>
                            {
                                text.Span("Donation Amount: ").SemiBold();
                                text.Span($"{donation.Currency} {donation.Amount:N2}").FontColor(Colors.Green.Darken2).FontSize(16);
                            });

                            x.Item().Text(text =>
                            {
                                text.Span("Donation Type: ").SemiBold();
                                text.Span(donation.DonationType);
                            });

                            x.Item().Text(text =>
                            {
                                text.Span("Date of Donation: ").SemiBold();
                                text.Span(donation.DonationDate.ToString("dd MMMM yyyy HH:mm"));
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Thank you for supporting our humanitarian relief efforts. ");
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            byte[] pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"TaxCertificate_{donation.Id}.pdf");
        }
    }
}