using ShippingAPI.ApplicationCore.Contracts.Services;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ShippingAPI.Infrastructure.Services;


public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _serializerOptions = 
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(json)) return default;
        return JsonSerializer.Deserialize<T>(json, _serializerOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value, _serializerOptions);
        var options = new DistributedCacheEntryOptions();
        if (expiry.HasValue)
            options.SetAbsoluteExpiration(expiry.Value);

        await _cache.SetStringAsync(key, json, options);
    }
    
    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
