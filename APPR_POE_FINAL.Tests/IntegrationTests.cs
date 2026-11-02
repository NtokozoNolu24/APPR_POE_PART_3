using Microsoft.EntityFrameworkCore;
using Xunit;
using APPR_POE_FINAL.Data;
using APPR_POE_FINAL.Models;

namespace APPR_POE_FINAL.Tests
{
    public class IntegrationTests
    {
        private static ApplicationDbContext NewContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Test_Donation_Saves_To_Database()
        {
            using var context = NewContext();
            context.Donations.Add(new Donation { Amount = 100, Currency = "ZAR", DonationType = "One-Time" });
            await context.SaveChangesAsync();
            var saved = await context.Donations.FirstOrDefaultAsync(d => d.Amount == 100);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Test_Multiple_Donations_Total_From_Database()
        {
            using var context = NewContext();
            context.Donations.Add(new Donation { Amount = 100, Currency = "ZAR", DonationType = "One-Time" });
            context.Donations.Add(new Donation { Amount = 300, Currency = "ZAR", DonationType = "Recurring" });
            await context.SaveChangesAsync();
            Assert.Equal(400, await context.Donations.SumAsync(d => d.Amount));
        }

        [Fact]
        public async Task Test_Donation_Can_Be_Updated()
        {
            using var context = NewContext();
            var donation = new Donation { Amount = 100, Currency = "ZAR", DonationType = "One-Time" };
            context.Donations.Add(donation);
            await context.SaveChangesAsync();

            donation.Amount = 750;
            await context.SaveChangesAsync();

            Assert.Equal(1, await context.Donations.CountAsync(d => d.Amount == 750));
        }

        [Fact]
        public async Task Test_Donation_Can_Be_Deleted()
        {
            using var context = NewContext();
            var donation = new Donation { Amount = 100, Currency = "ZAR", DonationType = "One-Time" };
            context.Donations.Add(donation);
            await context.SaveChangesAsync();

            context.Donations.Remove(donation);
            await context.SaveChangesAsync();

            Assert.Empty(context.Donations);
        }
    }
}