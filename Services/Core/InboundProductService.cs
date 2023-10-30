using AutoMapper;
using Data.DataAccess;

namespace Services.Core;

public interface IInboundProductService
{
}
public class InboundProductService : IInboundProductService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public InboundProductService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
}
