using Hangfire;

namespace Services.Core
{
    public interface IHangfireServices
    {
        string InventoryThresholdWarning(Guid productId, TimeSpan timeSpan);
        void DeleteJobClient(string jobId);
    }
    public class HangfireServices : IHangfireServices
    {
        private readonly IRecurringJobManager _recurringJob;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public HangfireServices(
            IRecurringJobManager recurringJobManager,
            IBackgroundJobClient backgroundJobClient
            )
        {
            _recurringJob = recurringJobManager;
            _backgroundJobClient = backgroundJobClient;
        }

        public string InventoryThresholdWarning(Guid blogTrId, TimeSpan timeSpan)
        {
            var jobId = _backgroundJobClient.Schedule<IProductService>(methodCall: (_) => _.GetInventories(blogTrId), delay: timeSpan);
            return jobId;
        }

        public void DeleteJobClient(string jobId)
        {
            _backgroundJobClient.Delete(jobId);
        }
    }
}
