using System.Collections.Immutable;

namespace AnovaTask.API.Storage;

public interface IReadingsStorage
{
    public Task<int> AddReadingsToDeviceAsync(int deviceId, IEnumerable<Reading> readings);

    public Task<ImmutableList<Reading>> GetReadingsFromWindowAsync(int deviceId, DateTime from, DateTime? to = null);
}

public class ReadingsStorage
{
    
}