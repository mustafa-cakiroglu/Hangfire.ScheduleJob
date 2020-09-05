using System;

namespace Hangfire.Api.Job
{
    public static class HangfireScheduler
    {
        public static void HangfireSchedulerJobs()
        {
            RecurringJob.RemoveIfExists(nameof(DealOfTheDayJobs));
            RecurringJob.AddOrUpdate<DealOfTheDayJobs>(nameof(DealOfTheDayJobs),
                Job => Job.Run(JobCancellationToken.Null),
                Cron.Daily, TimeZoneInfo.Local);

            //"0/3 * * * *"
        }
    }
}
