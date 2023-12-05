using Core.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    /// <summary>
    /// To install Redis visit https://github.com/microsoftarchive/redis/releases/
    /// </summary>
    public class RedisCacheService : IRedisCacheService
    {
        private  readonly IDatabase _db;
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value) && value!="{[]}")
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset? expirationTime)
        {
            var json = JsonSerializer.SerializeToUtf8Bytes(value);
            var isSet = await _db.StringSetAsync(key, json);

            return isSet;
        }
    }
}
