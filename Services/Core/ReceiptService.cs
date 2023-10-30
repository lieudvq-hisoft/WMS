using AutoMapper;
using Data.DataAccess;

namespace Services.Core;

public interface IReceiptService
{
}
public class ReceiptService : IReceiptService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ReceiptService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
}
