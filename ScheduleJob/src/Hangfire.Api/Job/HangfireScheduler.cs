using Hangfire.Api.Models;
using ServiceStack;
using System;

namespace Hangfire.Api.Job
{
    public static class HangfireScheduler
    {
        public static void HangfireSchedulerJobs(AppSettings appSettings)
        {
            RecurringJob.RemoveIfExists(nameof(DealOfTheDayJobs));
            RecurringJob.AddOrUpdate<DealOfTheDayJobs>(nameof(DealOfTheDayJobs),
                Job => Job.Run(JobCancellationToken.Null, appSettings),
                "0/2 * * * *", TimeZoneInfo.Local);

        }
    }
}
