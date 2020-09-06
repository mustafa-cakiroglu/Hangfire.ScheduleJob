using Hangfire.Api.Models;
using ServiceStack;
using ServiceStack.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hangfire.Api.Job
{
    public class DealOfTheDayJobs : IDealOfTheDayJobs
    {
        private AppSettings _appSettings;

        public Task Run(IJobCancellationToken token, AppSettings appSettings)
        {
            _appSettings = appSettings;
            return DummyData();
        }

        private Task DummyData()
        {
            using (IRedisClient client = new RedisClient())
            {
                for (int i = 1; i < 11; i++)
                {
                    var product = new Product
                    {
                        Id = i + 1,
                        ProductName = "Iphone" + (i + 2),
                        Category = "Cep Telefonu",
                        ExpireBeginDate = DateTime.Now,
                        ExpireEndDate = DateTime.Now.AddSeconds(i * 5),
                        ProductPrice = "1000" + (i * 500)
                    };

                    var productType = client.As<Product>();

                    var result = (product.ExpireEndDate - product.ExpireBeginDate);
                    productType.SetValue("Product" + product.Id, product, result);
                }
            }

            string EXPIRED_KEYS_CHANNEL = "__keyevent@0__:expired";
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(_appSettings.Redis.Host);
            ISubscriber subscriber = connection.GetSubscriber();
            subscriber.Subscribe(EXPIRED_KEYS_CHANNEL, (channel, key) =>
            {
                Console.WriteLine($"Hepsiburada cache: {key}");
            });

            return Task.CompletedTask;
        }
    }
}
