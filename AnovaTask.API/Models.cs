using System.Text.Json.Serialization;

namespace AnovaTask.API;

public record Device(
    [property:JsonPropertyName("device_id")] int DeviceId,
    string Name,
    string Location);
    
public record Reading(
    long timestamp,
    [property:JsonPropertyName("device_id")] int DeviceId,
    [property:JsonPropertyName("device_id")] string ReadingType,
    [property:JsonPropertyName("raw_value")] string RawValue);