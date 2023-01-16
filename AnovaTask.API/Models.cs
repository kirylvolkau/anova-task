using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnovaTask.API;

public record DeviceDto(
    [property:JsonPropertyName("device_id")]
    int DeviceId,
    
    [Required(AllowEmptyStrings=false)]
    string Name,
    
    [Required(AllowEmptyStrings=false)]
    string Location);

public record ReadingDto(
    long timestamp,
    
    [property:JsonPropertyName("device_id")]
    int DeviceId,
    
    [Required(AllowEmptyStrings=false)]
    [property:JsonPropertyName("device_id")]
    string ReadingType,
    
    [Required(AllowEmptyStrings=false)]
    [property:JsonPropertyName("raw_value")]
    string RawValue);