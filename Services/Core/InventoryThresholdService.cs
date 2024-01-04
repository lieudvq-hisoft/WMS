using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Data.Models;
using Hangfire;

namespace Services.Core
{
    public interface IInventoryThresholdService
    {
        Task<ResultModel> Get();
        Task<ResultModel> Update(InventoryThresholdUpdateModel model);
        Task<ResultModel> Create(InventoryThresholdCreateModel model);

    }
    public class InventoryThresholdService : IInventoryThresholdService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRecurringJobManager _recurringJob;
        public InventoryThresholdService(AppDbContext dbContext, IMapper mapper, IRecurringJobManager recurringJob)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _recurringJob = recurringJob;
        }

        public async Task<ResultModel> Get()
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var data = _dbContext.InventoryThresholds;
                result.Data = _mapper.ProjectTo<InventoryThresholdModel>(data!);
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        [Obsolete]
        public async Task<ResultModel> Update(InventoryThresholdUpdateModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var inventoryThreshold = _dbContext.InventoryThresholds.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
                if (inventoryThreshold == null)
                {
                    result.ErrorMessage = "InventoryThreshold not exists";
                    result.Succeed = false;
                    return result;
                }
                if (model.ThresholdQuantity != null)
                {
                    inventoryThreshold.ThresholdQuantity = (int)model.ThresholdQuantity;
                }
                if (model.CronExpression != null)
                {
                    inventoryThreshold.CronExpression = model.CronExpression;
                    _recurringJob.AddOrUpdate<IProductService>(
                        recurringJobId: "InventoryThresholdWarning",
                        methodCall: (_) => _.InventoryThresholdWarning(),
                        cronExpression: () => inventoryThreshold!.CronExpression != null ? inventoryThreshold.CronExpression : "10 15 * * *",
                        timeZone: TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
                    );
                }
                inventoryThreshold.DateUpdated = DateTime.Now;
                _dbContext.InventoryThresholds.Update(inventoryThreshold);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = inventoryThreshold;

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public async Task<ResultModel> Create(InventoryThresholdCreateModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var data = _mapper.Map<InventoryThresholdCreateModel, InventoryThreshold>(model);
                _dbContext.InventoryThresholds.Add(data);
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = data;

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
    }
}
