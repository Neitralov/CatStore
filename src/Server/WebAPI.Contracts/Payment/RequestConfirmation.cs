using System.Text.Json.Serialization;

namespace WebAPI.Contracts.Payment;

public record RequestConfirmation(string ReturnUrl, string Type = "redirect")
{
    [JsonPropertyName("return_url")]
    public string ReturnUrl { get; init; } = ReturnUrl;
    [JsonPropertyName("type")]
    public string Type { get; init; } = Type;
}