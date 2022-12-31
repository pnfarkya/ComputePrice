using ComputePrice.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ComputePrice.Data
{
    public class ComputPriceDbContext : DbContext
    {
        public ComputPriceDbContext(DbContextOptions<ComputPriceDbContext> options) : base(options) { }

        public DbSet<PriceModel> PriceModel => Set<PriceModel>();
    }
}
