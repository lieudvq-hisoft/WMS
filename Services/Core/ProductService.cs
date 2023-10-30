using AutoMapper;
using Data.DataAccess;

namespace Services.Core;

public interface IProductService
{
}
public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
}
