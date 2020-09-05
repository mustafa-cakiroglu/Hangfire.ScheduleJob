using System.Threading.Tasks;

namespace Hangfire.Api.Job
{
    public interface IDealOfTheDayJobs
    {
        Task Run(IJobCancellationToken token);
    }
}
