using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnovaTask.API;

public class DeviceDto
{
    [JsonPropertyName("device_id")]
    public int DeviceId { get; set; }

    [Required(AllowEmptyStrings = false)] public string Name { get; set; } = null!;

    [Required(AllowEmptyStrings = false)] public string Location { get; set; } = null!;
}

public class ReadingDto
{
    public long Timestamp { get; set; }
    
    [JsonPropertyName("device_id")] public int DeviceId { get; set; }

    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("device_id")]
    public string ReadingType { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("device_id")]
    public string RawValue { get; set; } = null!;
}