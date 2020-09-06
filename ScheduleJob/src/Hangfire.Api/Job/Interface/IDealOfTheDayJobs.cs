using Hangfire.Api.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Hangfire.Api.Job
{
    public interface IDealOfTheDayJobs
    {
        Task Run(IJobCancellationToken token, AppSettings appSettings);
    }
}
