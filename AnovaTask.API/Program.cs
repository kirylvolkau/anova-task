using AnovaTask.API.Storage;

var builder = WebApplication.CreateBuilder(args);

// ------- Add services to the container. ---------
builder.Services.AddControllers();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IDevicesStorage, DevicesStorage>();
builder.Services.AddScoped<IReadingsStorage, ReadingsStorage>();

// Add swagger-related services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ------ Configure the HTTP request pipeline. -------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
