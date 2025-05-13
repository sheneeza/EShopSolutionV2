using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using ShippingAPI.ApplicationCore.Contracts.Repositories;
using ShippingAPI.ApplicationCore.Contracts.Services;
using ShippingAPI.Infrastructure.Data;
using ShippingAPI.Infrastructure.Repositories;
using ShippingAPI.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var configuration = builder.Configuration;

// Configure Redis-backed IDistributedCache + your ICacheService
var redisOpts = configuration.GetSection("Redis");
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = redisOpts["Configuration"];
    options.InstanceName  = redisOpts["InstanceName"];
});
builder.Services.AddSingleton<ICacheService, RedisCacheService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ShippingDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("ShippingDatabase"));
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IShipperRepository, ShipperRepository>();
builder.Services.AddScoped<IShipperService, ShipperService>();
builder.Services.AddHttpClient("OrderService", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["OrderService:BaseUrl"]);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddCustomJwtAuthentication();
builder.Services.AddAuthorization(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
//}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.UseCors(policy =>
{
    policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}