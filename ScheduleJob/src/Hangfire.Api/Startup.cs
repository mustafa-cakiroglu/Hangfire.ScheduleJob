using Hangfire.Api.Helper;
using Hangfire.Api.Helper.Interface;
using Hangfire.Api.Job;
using Hangfire.Api.Models;
using Hangfire.Api.Services;
using Hangfire.Api.Services.Interface;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Hangfire.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public AppSettings _appSettings { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {
            _appSettings = this.Configuration.GetSection("AppSettings").Get<AppSettings>();

            services.AddHangfire(config =>
            {
                var migrationOptions = new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        Strategy = MongoMigrationStrategy.Drop,
                        BackupStrategy = MongoBackupStrategy.Collections
                    }
                };
                var settings = new MongoClientSettings();
                settings.Server = new MongoServerAddress(_appSettings.MongoConnectionStrings, 27017);
                config.UseMongoStorage(settings, "Hepsiburada", migrationOptions);
            });


            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = "RedisHepsiburada";
                options.Configuration = $"{_appSettings.Redis.Host}:{_appSettings.Redis.Port}";
            });

            services.Configure<AppSettings>(this.Configuration.GetSection("AppSettings"));

            services.AddControllers();
            IocConfiguration(services);
        }

        private static void IocConfiguration(IServiceCollection services)
        {
            services.AddScoped<IDealOfTheDayJobs, DealOfTheDayJobs>();
            services.AddScoped<IRedisConnectionManager, RedisConnectionManager>();
            services.AddScoped<IRedisService, RedisService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard();

            app.UseHangfireServer();

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            //TODO: Mustafa burada paramatre verılmeden gıdebılır mıyız kontrol et 
            HangfireScheduler.HangfireSchedulerJobs(_appSettings);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
