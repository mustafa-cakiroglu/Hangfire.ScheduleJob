using Hangfire.Api.Models;
using System.Threading.Tasks;

namespace Hangfire.Api.Job
{
    public interface IDealOfTheDayJobs
    {
        Task Run(IJobCancellationToken token, AppSettings appSettings);
    }
}
