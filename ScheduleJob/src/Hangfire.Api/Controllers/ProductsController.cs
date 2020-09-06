using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Api.Models;
using Hangfire.Api.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRedisService _redisService;

        public ProductsController(IRedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpGet("{Id}")]
        public Product GetProduct(string Id)
        {
            string cacheKey = "Product" + Id;
            return _redisService.GetById(cacheKey);
        }

        [HttpPost]
        public Product SaveProduct(Product product)
        {
            return _redisService.SetProduct(product);
        }
    }
}
