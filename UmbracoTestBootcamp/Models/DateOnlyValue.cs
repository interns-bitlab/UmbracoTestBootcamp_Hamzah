using System.Text.Json.Serialization;

namespace UmbracoTestBootcamp.Models;

public class DateOnlyValue
{
    [JsonPropertyName("date")]
    public DateTimeOffset Date { get; init; }
}
