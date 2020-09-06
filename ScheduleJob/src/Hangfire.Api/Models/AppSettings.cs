namespace Hangfire.Api.Models
{
    public class AppSettings
    {
        public string MongoConnectionStrings { get; set; }
        public RedisConnectionModel Redis { get; set; }

    }
}
