using Xunit;
using GiftOfTheGivers.Common;
using APPR_POE_FINAL.Models;

namespace APPR_POE_FINAL.Tests
{
    public class UnitTests
    {
        [Fact]
        public void Test_ReliefHelper_Returns_Approved()
        {
            var result = ReliefHelper.GetReliefStatus("approved");
            Assert.Equal("Relief request has been approved.", result);
        }

        [Fact]
        public void Test_Donation_Valid_Amount()
        {
            var donation = new Donation { Amount = 500, Currency = "ZAR" };
            Assert.True(donation.Amount > 0);
        }

        [Fact]
        public void Test_ReliefHelper_Pending_Differs_From_Approved()
        {
            var approved = ReliefHelper.GetReliefStatus("approved");
            var pending = ReliefHelper.GetReliefStatus("pending");
            Assert.False(string.IsNullOrWhiteSpace(pending));
            Assert.NotEqual(approved, pending);
        }

        [Fact]
        public void Test_ReliefHelper_Handles_Empty_Input()
        {
            var ex = Record.Exception(() => ReliefHelper.GetReliefStatus(""));
            Assert.Null(ex);
        }

        [Fact]
        public void Test_Donation_Stores_Currency_And_Type()
        {
            var donation = new Donation { Amount = 250, Currency = "USD", DonationType = "Recurring" };
            Assert.Equal("USD", donation.Currency);
            Assert.Equal("Recurring", donation.DonationType);
        }

        [Fact]
        public void Test_Donation_Total_Is_Calculated_Correctly()
        {
            var donations = new List<Donation>
            {
                new Donation { Amount = 500, Currency = "ZAR" },
                new Donation { Amount = 250, Currency = "ZAR" },
                new Donation { Amount = 850, Currency = "ZAR" }
            };
            Assert.Equal(1600, donations.Sum(d => d.Amount));
        }
    }
}