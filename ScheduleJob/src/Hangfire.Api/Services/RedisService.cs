using Hangfire.Api.Models;
using Hangfire.Api.Services.Interface;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace Hangfire.Api.Services
{
    public class RedisService : IRedisService
    {
        public List<Product> GetAll(string cachekey)
        {
            throw new NotImplementedException();
        }

        public Product GetById(string cachekey)
        {
            using (IRedisClient client = new RedisClient())
            {
                var redisdata = client.Get<Product>(cachekey);

                return redisdata;
            }
        }

        public Product SetProduct(Product product)
        {
            using (IRedisClient client = new RedisClient())
            {
                var cachedata = client.As<Product>();
                cachedata.SetValue("Product" + product.Id, product, TimeSpan.FromSeconds(50));

                var redisdata = client.Get<Product>("Product" + product.Id);

                return redisdata;
            }
        }
    }
}
