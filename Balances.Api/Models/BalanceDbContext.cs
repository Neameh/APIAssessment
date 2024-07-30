using Microsoft.EntityFrameworkCore;

namespace Balances.Api.Models
{

    public class BalanceDbContext : DbContext
    {
        public BalanceDbContext(DbContextOptions<BalanceDbContext> options) : base(options)
        {
        }

        public DbSet<Balance> Balances { get; set; }
    }
}

