using System.Reflection;
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
builder.Services.AddSwaggerGen(options=> {
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// ------ Configure the HTTP request pipeline. -------
app.UseSwagger();
app.UseSwaggerUI();

// docker and https is always fun! that's why I turned off https.
// in production system this would still probably be turned off, because of the proxy sitting right before the service
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
