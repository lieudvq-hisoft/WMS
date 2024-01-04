using Hangfire;

namespace Services.Core
{
    public interface IHangfireServices
    {
        void InventoryThresholdWarning();
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

        [Obsolete]
        public void InventoryThresholdWarning()
        {
            _recurringJob.AddOrUpdate<IProductService>(
                recurringJobId: "InventoryThresholdWarning",
                methodCall: (_) => _.InventoryThresholdWarning(),
                cronExpression: () => "10 15 * * *",
                timeZone: TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
            );
        }

        public void DeleteJobClient(string jobId)
        {
            _backgroundJobClient.Delete(jobId);
        }
    }
}
