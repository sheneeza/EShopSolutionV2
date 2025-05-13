using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using PromotionsAPI.ApplicationCore.Contracts.Repositories;
using PromotionsAPI.ApplicationCore.Contracts.Services;
using PromotionsAPI.Infrastructure.Data;
using PromotionsAPI.Infrastructure.Repositories;
using PromotionsAPI.Infrastructure.Services;
using PromotionsAPI.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PromotionsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("PromotionsDatabase"));
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddSingleton(sp =>
{
    // reads from appsettings.json "ConnectionStrings:ServiceBus"
    var connStr = builder.Configuration.GetConnectionString("ServiceBus");
    return new PromotionEventPublisher(connStr);
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