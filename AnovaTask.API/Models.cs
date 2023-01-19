using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnovaTask.API;

/// <summary>
/// DTO for transferring device data between layers (one for all of them).
/// </summary>
public class DeviceDto
{
    /// <summary>
    /// Id of the device.
    /// </summary>
    [Required]
    [JsonPropertyName("device_id")]
    public int DeviceId { get; set; }

    /// <summary>
    /// Name of the device.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Location string.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Location { get; set; } = null!;
}

/// <summary>
/// DTO for transferring readings data between layers (one for all of them).
/// </summary>
public class ReadingDto
{
    /// <summary>
    /// Timestamp (in seconds from unix epoch) of the reading.
    /// 
    /// The <see cref="Range"/> constraint.
    /// is limited on the right side by the amount of milliseconds in one year - so we can know
    /// that out timestamp is in seconds.
    /// </summary>
    [Required]
    [Range(0L, 31_536_000_000L)]
    public long Timestamp { get; set; }

    /// <summary>
    /// Device ID referencing <see cref="DeviceDto.DeviceId"/>.
    /// </summary>
    [Required]
    [JsonPropertyName("device_id")]
    public int DeviceId { get; set; }

    /// <summary>
    /// Type of the reading.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("reading_type")]
    public string ReadingType { get; set; } = null!;

    /// <summary>
    /// Raw value of the reading (represented as string to cover all possible incoming values).
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("raw_value")]
    public string RawValue { get; set; } = null!;
}