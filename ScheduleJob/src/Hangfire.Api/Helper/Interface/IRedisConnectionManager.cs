using Hangfire.Api.Models;

namespace Hangfire.Api.Helper.Interface
{
    public interface IRedisConnectionManager
    {
        RedisConnectionModel RedisConnection { get; }
    }
}
