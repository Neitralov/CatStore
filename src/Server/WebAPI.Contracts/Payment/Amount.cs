using System.Text.Json.Serialization;

namespace WebAPI.Contracts.Payment;

public record Amount(decimal Value, string Currency = "RUB")
{
    [JsonPropertyName("value")]
    public decimal Value { get; init; } = Value;
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = Currency;
}