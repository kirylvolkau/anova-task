using AnovaTask.API.Storage;

var builder = WebApplication.CreateBuilder(args);

// ------- Add services to the container. ---------
builder.Services.AddControllers();

// Configure storage
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IDevicesStorage, DevicesStorage>();
builder.Services.AddScoped<IReadingsStorage, ReadingsStorage>();

// Add swagger-related services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ------ Configure the HTTP request pipeline. -------
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
