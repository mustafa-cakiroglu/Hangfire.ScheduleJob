using Hangfire.Api.Models;
using ServiceStack.Redis;
using StackExchange.Redis;
using System;
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
            using (var client = new RedisClient())
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

            string expiredKeysChannel = "__keyevent@0__:expired";
            var connection = ConnectionMultiplexer.Connect(_appSettings.Redis.Host);
            var subscriber = connection.GetSubscriber();
            subscriber.Subscribe(expiredKeysChannel, (channel, key) =>
            {
                Console.WriteLine($"Hepsiburada cache: {key}");
            });

            return Task.CompletedTask;
        }
    }
}
