namespace GiftOfTheGivers.Common
{
    public class ReliefHelper
    {
        public static string GetReliefStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return "Status not provided";

            return status.Trim().ToLower() switch
            {
                "pending" => "Relief request is pending.",
                "approved" => "Relief request has been approved.",
                "completed" => "Relief request has been completed.",
                _ => "Unknown relief status."
            };
        }
    }
}