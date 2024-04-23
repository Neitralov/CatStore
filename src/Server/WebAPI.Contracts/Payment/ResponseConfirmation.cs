using System.Text.Json.Serialization;

namespace WebAPI.Contracts.Payment;

public record ResponseConfirmation(string ConfirmationUrl, string Type)
{
    [JsonPropertyName("confirmation_url")]
    public string ConfirmationUrl { get; init; } = ConfirmationUrl;
}