using System;

namespace Hangfire.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string Category { get; set; }
        public DateTime ExpireBeginDate { get; set; }
        public DateTime ExpireEndDate { get; set; }
    }
}
