using Hangfire.Api.Job;
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

        public void ConfigureServices(IServiceCollection services)
        {
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
                settings.Server = new MongoServerAddress(Configuration["MongoConnectionStrings"], 27017);
                config.UseMongoStorage(settings, "Hepsiburada", migrationOptions);
            });

            services.AddControllers();
            IocConfiguration(services);
        }

        private static void IocConfiguration(IServiceCollection services)
        {
            services.AddScoped<IDealOfTheDayJobs, DealOfTheDayJobs>();

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
            HangfireScheduler.HangfireSchedulerJobs();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
