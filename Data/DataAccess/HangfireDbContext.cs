using Microsoft.EntityFrameworkCore;

namespace Data.DataAccess;

public class HangfireDbContext : DbContext
{
    public HangfireDbContext(DbContextOptions<HangfireDbContext> options) : base(options)
    {
    }
}
