using Data.DataAccess;
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
        private readonly AppDbContext _dbContext;
        public HangfireServices(
            IRecurringJobManager recurringJobManager,
            IBackgroundJobClient backgroundJobClient,
            AppDbContext dbContext
            )
        {
            _recurringJob = recurringJobManager;
            _backgroundJobClient = backgroundJobClient;
            _dbContext = dbContext;
        }

        [Obsolete]
        public void InventoryThresholdWarning()
        {
            var inventoryThreshold = _dbContext.InventoryThresholds.FirstOrDefault();
            _recurringJob.AddOrUpdate<IProductService>(
                recurringJobId: "InventoryThresholdWarning",
                methodCall: (_) => _.InventoryThresholdWarning(),
                cronExpression: () => inventoryThreshold!.CronExpression != null ? inventoryThreshold.CronExpression : "10 15 * * *",
                timeZone: TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
            );
        }

        public void DeleteJobClient(string jobId)
        {
            _backgroundJobClient.Delete(jobId);
        }
    }
}
