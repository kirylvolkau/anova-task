using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnovaTask.API;

public class DeviceDto
{
    [JsonPropertyName("device_id")]
    public int DeviceId { get; init; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    public string Location { get; init; } = null!;
}

public class ReadingDto
{
    [Range(0L, 31_536_000_000L)]
    public long Timestamp { get; set; }
    
    [JsonPropertyName("device_id")]
    public int DeviceId { get; init; }

    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("device_id")]
    public string ReadingType { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("device_id")]
    public string RawValue { get; init; } = null!;
}