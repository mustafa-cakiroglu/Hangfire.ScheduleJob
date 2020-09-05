using System.Threading.Tasks;

namespace Hangfire.Api.Job
{
    public class DealOfTheDayJobs : IDealOfTheDayJobs
    {
        public Task Run(IJobCancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
