using System.Collections.Immutable;

namespace AnovaTask.API.Storage;

public interface IReadingsStorage
{
    public Task<int?> AddReadingsToDeviceAsync(int deviceId, IEnumerable<ReadingDto> readings);

    public Task<ImmutableList<ReadingDto>?> GetReadingsFromWindowAsync(int deviceId, long from, long to);
}

public class ReadingsStorage : IReadingsStorage
{
    private readonly DapperContext _dapperContext;

    public ReadingsStorage(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    
    public Task<int?> AddReadingsToDeviceAsync(int deviceId, IEnumerable<ReadingDto> readings)
    {
        throw new NotImplementedException();
    }

    public Task<ImmutableList<ReadingDto>?> GetReadingsFromWindowAsync(int deviceId, long from, long to)
    {
        throw new NotImplementedException();
    }
}