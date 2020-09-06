using Hangfire.Api.Models;
using System.Collections.Generic;

namespace Hangfire.Api.Services.Interface
{
    public interface IRedisService
    {
        List<Product> GetAll(string cachekey);
        Product GetById(string cachekey);
        Product SetProduct(Product product);
    }
}
