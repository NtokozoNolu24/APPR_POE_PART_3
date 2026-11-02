using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GiftoftheGivers.Functions;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger) => _logger = logger;

    public record DonationRequest(string? DonorName, decimal Amount);

    [Function("GenerateDonationReceipt")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        DonationRequest? data;
        try
        {
            data = await JsonSerializer.DeserializeAsync<DonationRequest>(
                req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException)
        {
            return new BadRequestObjectResult("Invalid JSON body.");
        }

        if (data is null || string.IsNullOrWhiteSpace(data.DonorName) || data.Amount <= 0)
            return new BadRequestObjectResult("donorName and a positive amount are required.");

        var receiptNumber = $"GOTG-{DateTime.UtcNow:yyyyMMddHHmmss}";
        _logger.LogInformation("Receipt {Receipt} generated for {Donor}, amount {Amount}",
            receiptNumber, data.DonorName, data.Amount);

        return new OkObjectResult(new
        {
            receiptNumber,
            donorName = data.DonorName,
            amount = data.Amount,
            date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm"),
            message = "Thank you for your donation to Gift of the Givers."
        });
    }
}