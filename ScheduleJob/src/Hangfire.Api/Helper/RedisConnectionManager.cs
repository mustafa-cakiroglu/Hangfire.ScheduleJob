using Hangfire.Api.Helper.Interface;
using Hangfire.Api.Models;
using Microsoft.Extensions.Options;

namespace Hangfire.Api.Helper
{
    public class RedisConnectionManager : IRedisConnectionManager
    {
        public RedisConnectionModel RedisConnection { get; }

        public RedisConnectionManager(IOptions<AppSettings> appsettings)
        {
            if (RedisConnection == null)
            {
                RedisConnection = new RedisConnectionModel();
            }

            RedisConnection.Host = appsettings.Value.Redis.Host;
            RedisConnection.Port = appsettings.Value.Redis.Port;
        }
    }
}
