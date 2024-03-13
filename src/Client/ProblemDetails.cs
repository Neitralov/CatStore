using System.Text.Json.Serialization;

namespace Client;

public class ProblemDetails
{
    [JsonPropertyName("errors")]
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    
    public string GetFirstErrorMessage() => Errors.Values.First().First();
}