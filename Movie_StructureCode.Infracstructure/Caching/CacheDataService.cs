using Movie_StructureCode.Application.Abstractions.Services.Cache;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Movie_StructureCode.Infracstructure.Caching
{
    public class CacheDataService : ICacheDataService
    {
        private readonly IDatabase _redis;


        public CacheDataService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();   
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _redis.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;
            
            return JsonSerializer.Deserialize<T>(value!);
        }

        public async Task RemoveAsync(string key)
        {
            await _redis.KeyDeleteAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration)
        {
            var json = JsonSerializer.Serialize(value);

            if (expiration.HasValue)
            {
                await _redis.StringSetAsync(key, json, expiry: expiration.Value);
            }
            else
            {
                await _redis.StringSetAsync(key, json);
            }
        }
    }
}
