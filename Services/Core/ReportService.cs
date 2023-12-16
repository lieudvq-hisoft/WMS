using System;
using AutoMapper;
using Confluent.Kafka;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Model;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Utils;

namespace Services.Core;

public interface IReportService
{
    Task<ResultModel> GetWeeklyReport();

}
public class ReportService : IReportService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IProducer<Null, string> _producer;

    public ReportService(AppDbContext dbContext, IMapper mapper, IProducer<Null, string> producer)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _producer = producer;
    }

    public async Task<ResultModel> GetWeeklyReport()
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var date = DateTime.Now;
            var receipts = _dbContext.Receipt.Where(_ =>
                _.DateCreated.Year == date.Year &&
                _.DateCreated.Month == date.Month &&
                !_.IsDeleted).ToList();
            var pickingRequests = _dbContext.PickingRequest.Where(_ =>
                _.DateCreated.Year == date.Year &&
                _.DateCreated.Month == date.Month &&
                !_.IsDeleted).ToList();
            result.Succeed = true;
            result.Data = new ReportOfMonthModel
            {
                ReceiptCompleted = receipts.Where(_ => _.Status == ReceiptStatus.Completed).Sum(_ => _.Quantity),
                ReceiptPending = receipts.Where(_ => _.Status == ReceiptStatus.Pending).Sum(_ => _.Quantity),
                PickingRequestCompleted = pickingRequests.Where(_ => _.Status == PickingRequestStatus.Completed).Sum(_ => _.Quantity),
                PickingRequestPending = pickingRequests.Where(_ => _.Status == PickingRequestStatus.Pending).Sum(_ => _.Quantity)
            };
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
