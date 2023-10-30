using AutoMapper;
using Data.DataAccess;

namespace Services.Core;

public interface ISupplierService
{
}
public class SupplierService : ISupplierService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public SupplierService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
}
